using AutoMapper;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using MyTrap.Business.Mobile.Contracts;
using MyTrap.Framework.Properties;
using MyTrap.Framework.Utils;
using MyTrap.Model.Framework;
using MyTrap.Model.Mobile.Request;
using MyTrap.Model.Mobile.Result;
using MyTrap.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTrap.Business.Mobile
{
    public class TrapBusiness : ITrapBusiness
    {
        public async Task<TrapResult> GetById(string trapId)
        {
            return await AppRepository.Trap.GetById(trapId);
        }

        public async Task<TrapResult> GetByNameKey(string nameKey)
        {
            return await AppRepository.Trap.GetByNameKey(nameKey);
        }

        public async Task<UserResult> ArmTrap(ArmedTrapRequest request)
        {
            bool isLocationValid = request.Latitude != 0 && request.Longitude != 0;

            UserResult user = null;

            if (isLocationValid)
            {
                TrapResult trap = await AppBusiness.Trap.GetByNameKey(request.NameKey);

                if (trap != null)
                {
                    user = await AppBusiness.User.GetById(request.UserId);

                    if (user != null)
                    {
                        if (user.Traps != null && user.Traps.Count > 0)
                        {
                            bool hasTrapToSet = false;

                            foreach (UserTrapResult userTrap in user.Traps)
                            {
                                if (userTrap.NameKey == request.NameKey && userTrap.Amount > 0)
                                {
                                    hasTrapToSet = true;
                                    break;
                                }
                            }

                            if (hasTrapToSet)
                            {
                                request.Date = DateTime.UtcNow;

                                await AppRepository.Trap.InsertArmedTrap(request);

                                UserRequest userRequest = Mapper.Map<UserRequest>(user);

                                await AppBusiness.User.RemoveTrap(userRequest, request.NameKey);

                                user = await AppBusiness.User.GetById(request.UserId);
                            }
                            else
                            {
                                throw new MyTrapBusinessException(Resources.trap_armed_user_no_has);
                            }
                        }
                        else
                        {
                            throw new MyTrapBusinessException(Resources.trap_armed_user_no_traps);
                        }
                    }
                }
            }

            if (!isLocationValid)
            {
                throw new MyTrapBusinessException(Resources.trap_armed_location_invalid);
            }

            return user;
        }

        public async Task AddPositionProcessQueue(PositionRequest request)
        {
            if (request.Latitude != 0 && request.Longitude != 0)
            {
                await AddPositionQueue(request.UserId, request.Latitude, request.Longitude);
            }
        }

        public async Task ProcessDisarmedTrap(string userId, string disarmedTrapId)
        {
            UserResult user = await AppBusiness.User.GetById(userId);
            ArmedTrapResult disarmedTrap = await AppRepository.Trap.GetArmedTrapById(disarmedTrapId);

            UserResult userInjured = user;
            UserResult userOwnerTrap = await AppBusiness.User.GetById(disarmedTrap.UserId);

            if (userOwnerTrap != null)
            {
                TrapResult trap = await AppBusiness.Trap.GetByNameKey(disarmedTrap.NameKey);

                UserRequest userOwnerRequest = Mapper.Map<UserRequest>(userOwnerTrap);
                UserRequest userInjuredRequest = Mapper.Map<UserRequest>(userInjured);

                await AppBusiness.User.AddPoints(userOwnerRequest, trap.Points);

                /* Deixar de forma dinamica futuramente */
                var trapOwned = await AppBusiness.Trap.GetByNameKey("BEAR_TRAP");

                var trapUserOwned = new UserTrapRequest();

                trapUserOwned.TrapId = trapOwned.Id.ToString();
                trapUserOwned.Amount = 5;

                AppBusiness.User.AddTrap(userOwnerRequest, trapUserOwned);

                await AppBusiness.User.Save(userOwnerRequest);

                await SendNotificationToUserInjured(userInjuredRequest, userOwnerRequest, disarmedTrap);

                await SendNotificationToUserOwnerTrap(userInjuredRequest, userOwnerRequest, disarmedTrap, trap.Points);
            }
        }

        public async Task<List<ArmedTrapResult>> ListArmedTraps(UserRequest request)
        {
            List<ArmedTrapResult> response = await AppRepository.Trap.ListArmedTrapsByUser(request.Id);

            List<TrapResult> availableTraps = new List<TrapResult>();

            if (response != null)
            {
                foreach (ArmedTrapResult trap in response)
                {
                    TrapResult availableTrap = availableTraps.Where(obj => obj.NameKey == trap.NameKey).FirstOrDefault();

                    if (availableTrap == null)
                    {
                        availableTrap = await AppBusiness.Trap.GetByNameKey(trap.NameKey);

                        availableTraps.Add(availableTrap);
                    }
                }
            }

            return response;
        }

        private async Task SendNotificationToUserOwnerTrap(UserRequest userInjured, UserRequest userOwnerTrap, ArmedTrapResult disarmedTrap, int points)
        {
            var message = StringUtils.GetStringForCulture("you_caught_user_trap", userOwnerTrap.Language);

            message = string.Format(message, userInjured.Name);

            string otherUserImage = string.Empty;

            if (userInjured.ProfilePicture != null && !string.IsNullOrEmpty(userInjured.ProfilePicture.Url))
            {
                otherUserImage = userInjured.ProfilePicture.Url;
            }

            await new NotificationBusiness().SendNotificationTrapDisarmed(userOwnerTrap.Email, message, true, points, disarmedTrap.NameKey, disarmedTrap.Latitude, disarmedTrap.Longitude, userInjured.Name, otherUserImage);
        }

        private async Task SendNotificationToUserInjured(UserRequest userInjured, UserRequest userOwnerTrap, ArmedTrapResult disarmedTrap)
        {
            var message = StringUtils.GetStringForCulture("you_were_caught_trap", userInjured.Language);

            message = string.Format(message, userOwnerTrap.Name);

            string otherUserImage = string.Empty;

            if (userOwnerTrap.ProfilePicture != null && !string.IsNullOrEmpty(userOwnerTrap.ProfilePicture.Url))
            {
                otherUserImage = userOwnerTrap.ProfilePicture.Url;
            }

            await new NotificationBusiness().SendNotificationTrapDisarmed(userInjured.Email, message, false, 0, disarmedTrap.NameKey, disarmedTrap.Latitude, disarmedTrap.Longitude, userOwnerTrap.Name, otherUserImage);
        }

        private async Task AddPositionQueue(string userId, float latitude, float longitude)
        {
            string storageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            CloudQueue queue = queueClient.GetQueueReference("positionqueue");

            await queue.CreateIfNotExistsAsync();

            var request = new { userId = userId, latitude = latitude, longitude = longitude };

            string json = JsonConvert.SerializeObject(request);

            CloudQueueMessage message = new CloudQueueMessage(json);

            await queue.AddMessageAsync(message);
        }
    }
}