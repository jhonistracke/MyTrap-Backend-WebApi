using MyTrap.Business.Mobile.Contracts;
using MyTrap.Framework.Properties;
using MyTrap.Model.Cache;
using MyTrap.Model.Enums;
using MyTrap.Model.Framework;
using MyTrap.Model.Mobile.Request;
using MyTrap.Model.Mobile.Result;
using MyTrap.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrap.Business.Mobile
{
    public class UserBusiness : IUserBusiness
    {
        public async Task<UserResult> Save(UserRequest user)
        {
            if (user.RegisterType == ERegisterType.FACEBOOK)
            {
                UserResult actualUserResponse = await AppRepository.User.GetByEmail(user.Email);

                if (actualUserResponse == null)
                {
                    user.ProfilePicture = AppBusiness.Blob.InsertUserImage(user.ProfilePicture);

                    await AddInitialTraps(user);

                    await AppRepository.User.Insert(user);
                }
                else
                {
                    if (NeedUpdateProfileImage(actualUserResponse, user))
                    {
                        ImageRequest image = AppBusiness.Blob.InsertUserImage(user.ProfilePicture);

                        actualUserResponse.ProfilePicture = new ImageResult(image);

                        await AppRepository.User.UpdateProfilePicture(actualUserResponse.Id, image);
                    }

                    if (actualUserResponse.Name != user.Name)
                    {
                        await AppRepository.User.UpdateName(actualUserResponse.Id, user.Name);
                    }

                    if (user.Traps != null && user.Traps.Count > 0)
                    {
                        await AppRepository.User.UpdateTraps(actualUserResponse.Id, user.Traps);
                    }

                    if (!string.IsNullOrEmpty(user.AppRegistration) && actualUserResponse.AppRegistration != user.AppRegistration)
                    {
                        await AppRepository.User.UpdateAppRegistration(user.Email, user.AppRegistration);

                        await AppBusiness.Notification.Register(user.Email, (int)user.Platform, user.AppRegistration);
                    }

                    if (user.Platform != actualUserResponse.Platform)
                    {
                        await AppRepository.User.UpdatePlatformId(user.Email, (int)user.Platform);
                    }

                    if (!string.IsNullOrEmpty(user.Language) && actualUserResponse.Language != user.Language)
                    {
                        await AppRepository.User.UpdateLanguage(user.Email, user.Language);
                    }

                    if (!string.IsNullOrEmpty(user.TimeZone) && actualUserResponse.TimeZone != user.TimeZone)
                    {
                        await AppRepository.User.UpdateTimeZone(user.Email, user.TimeZone);
                    }

                    user.ProfilePicture = new ImageRequest(actualUserResponse.ProfilePicture);
                }
            }

            await UpdateUserCache(user.Email);

            return await AppRepository.User.GetByEmail(user.Email);
        }

        private bool NeedUpdateProfileImage(UserResult actualUser, UserRequest updatedUser)
        {
            try
            {
                return actualUser.ProfilePicture != null && updatedUser.ProfilePicture != null && actualUser.ProfilePicture.OriginUrl != updatedUser.ProfilePicture.Url && (!string.IsNullOrEmpty(actualUser.ProfilePicture.Url) && actualUser.ProfilePicture.Url != updatedUser.ProfilePicture.Url);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task AddPoints(UserRequest user, int points)
        {
            await AppRepository.User.AddPoints(user.Email, points);

            await UpdateUserCache(user.Email);
        }

        private async Task AddInitialTraps(UserRequest user)
        {
            UserTrapRequest userTrap = new UserTrapRequest();

            userTrap.TrapId = await AppBusiness.Parameter.GetValue(EParameter.ID_TRAP_NEW_REGISTER);
            userTrap.Amount = Convert.ToInt32(await AppBusiness.Parameter.GetValue(EParameter.AMOUNT_TRAP_NEW_REGISTER));

            AddTrap(user, userTrap);
        }

        private async Task IsEmailDuplicate(UserRequest user)
        {
            UserResult actualUser = await AppRepository.User.GetByEmail(user.Email);

            if (actualUser != null && actualUser.Id != user.Id)
            {
                throw new MyTrapBusinessException(Resources.email_duplicate);
            }
        }

        public async Task<UserResult> Login(UserRequest request)
        {
            UserResult user = await AppBusiness.User.Save(request);

            string token = GetNewToken();

            user.Token = token;

            await UpdateToken(user.Id, token);

            await UpdateUserCache(user.Email, token);

            return user;
        }

        private async Task UpdateToken(string userId, string token)
        {
            //Remove old Token
            string userJson = await AppRepository.RedisCache.GetValueAsync(new UserResult() { Id = userId }.CacheKey);

            if (!string.IsNullOrEmpty(userJson))
            {
                UserResult user = JsonConvert.DeserializeObject<UserResult>(userJson);

                if (!string.IsNullOrEmpty(user.Token))
                {
                    await AppRepository.RedisCache.RemoveKeyAsync(new UserTokenCache() { Token = user.Token }.CacheKey);
                }
            }

            UserTokenCache userTokenCache = new UserTokenCache() { UserId = userId, Token = token };

            await AppRepository.RedisCache.SetValueAsync(userTokenCache.CacheKey, JsonConvert.SerializeObject(userTokenCache), -1);
        }

        private string GetNewToken()
        {
            return Guid.NewGuid().ToString().ToLower();
        }

        public void AddTrap(UserRequest user, UserTrapRequest userTrap)
        {
            bool hasTrap = false;

            if (user.Traps == null)
            {
                user.Traps = new List<UserTrapRequest>();
            }

            foreach (UserTrapRequest trapUserActual in user.Traps)
            {
                if (trapUserActual.TrapId == userTrap.TrapId)
                {
                    trapUserActual.Amount += userTrap.Amount;

                    hasTrap = true;
                }
            }

            if (!hasTrap)
            {
                user.Traps.Add(userTrap);
            }
        }

        public async Task RemoveTrap(UserRequest user, string nameKey)
        {
            foreach (UserTrapRequest trapUserActual in user.Traps)
            {
                if (trapUserActual.NameKey == nameKey)
                {
                    trapUserActual.Amount -= 1;
                    break;
                }
            }

            await AppRepository.User.UpdateTraps(user.Id, user.Traps);

            await UpdateUserCache(user.Email);
        }

        public async Task<UserResult> GetById(string id)
        {
            string userJson = await AppRepository.RedisCache.GetValueAsync(new UserResult() { Id = id }.CacheKey);

            if (string.IsNullOrEmpty(userJson))
            {
                UserResult user = await AppRepository.User.GetById(id);

                if (user != null)
                {
                    await UpdateUserCache(user.Email);

                    userJson = await AppRepository.RedisCache.GetValueAsync(new UserResult() { Id = id }.CacheKey);
                }
            }

            return JsonConvert.DeserializeObject<UserResult>(userJson);
        }

        public async Task<bool> UpdateUserCache(string email, string token = "")
        {
            bool result = false;

            UserResult user = await AppRepository.User.GetByEmail(email);

            if (!string.IsNullOrEmpty(token))
            {
                //Se token novo, sobrescreve
                user.Token = token;
            }
            else
            {
                //Se nao tem token, utiliza token do cache
                string userCacheJson = await AppRepository.RedisCache.GetValueAsync(new UserResult() { Id = user.Id }.CacheKey);

                if (!string.IsNullOrEmpty(userCacheJson))
                {
                    UserResult userCache = JsonConvert.DeserializeObject<UserResult>(userCacheJson);

                    user.Token = userCache.Token;
                }
            }

            string userJson = JsonConvert.SerializeObject(user);

            await AppRepository.RedisCache.SetValueAsync(user.CacheKey, userJson, -1);

            return result;
        }

        public async Task<UserResult> GetByTokenAsync(string token)
        {
            string userTokenCacheJson = AppRepository.RedisCache.GetValue(new UserTokenCache() { Token = token }.CacheKey);

            if (!string.IsNullOrEmpty(userTokenCacheJson))
            {
                UserTokenCache userTokenCache = JsonConvert.DeserializeObject<UserTokenCache>(userTokenCacheJson);

                string userJson = await AppRepository.RedisCache.GetValueAsync(new UserResult() { Id = userTokenCache.UserId }.CacheKey);

                return JsonConvert.DeserializeObject<UserResult>(userJson);
            }
            else
            {
                return null;
            }
        }

        public UserResult GetByToken(string token)
        {
            string userTokenCacheJson = AppRepository.RedisCache.GetValue(new UserTokenCache() { Token = token }.CacheKey);

            if (!string.IsNullOrEmpty(userTokenCacheJson))
            {
                UserTokenCache userTokenCache = JsonConvert.DeserializeObject<UserTokenCache>(userTokenCacheJson);

                string userJson = AppRepository.RedisCache.GetValue(new UserResult() { Id = userTokenCache.UserId }.CacheKey);

                return JsonConvert.DeserializeObject<UserResult>(userJson);
            }
            else
            {
                return null;
            }
        }

        public async Task UpdateRegistrationId(string email, string appRegistrationId, EPlatform platform)
        {
            UserResult actualUserResponse = await AppRepository.User.GetByEmail(email);

            if (!string.IsNullOrEmpty(appRegistrationId) && actualUserResponse.AppRegistration != appRegistrationId)
            {
                await AppRepository.User.UpdateAppRegistration(email, appRegistrationId);

                await AppBusiness.Notification.Register(email, (int)platform, appRegistrationId);
            }

            if (platform != actualUserResponse.Platform)
            {
                await AppRepository.User.UpdatePlatformId(email, (int)platform);
            }
        }
    }
}