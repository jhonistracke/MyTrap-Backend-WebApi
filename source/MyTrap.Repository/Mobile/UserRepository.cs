using MyTrap.Model.Enums;
using MyTrap.Model.Mobile.Request;
using MyTrap.Model.Mobile.Result;
using MyTrap.Repository.Mobile.Contracts;
using MyTrap.Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MyTrap.Repository.Mobile
{
    public class UserRepository : IUserRepository
    {
        public async Task AddPoints(string email, int points)
        {
            User actualUser = await AppRepository.EntitiesContext.Users.FirstOrDefaultAsync(obj => obj.Email == email);

            actualUser.UpdateDate = DateTime.UtcNow;
            actualUser.Points += points;

            await AppRepository.EntitiesContext.SaveChangesAsync();
        }

        public async Task<UserResult> GetByEmail(string email)
        {
            User user = await AppRepository.EntitiesContext.Users.Where(obj => obj.Email == email)
                .Include(obj => obj.Traps)
                .Include(obj => obj.Traps.Select(t => t.Trap))
                .Include(obj => obj.ProfilePicture).FirstOrDefaultAsync();

            return Parse(user);
        }

        public async Task<UserResult> GetById(string id)
        {
            User user = await AppRepository.EntitiesContext.Users.Where(obj => obj.Id == new Guid(id))
                .Include(obj => obj.Traps)
                .Include(obj => obj.Traps.Select(t => t.Trap))
                .Include(obj => obj.ProfilePicture).FirstOrDefaultAsync();

            return Parse(user);
        }

        public async Task Insert(UserRequest request)
        {
            User user = Parse(request);

            user.CreateDate = DateTime.UtcNow;

            AppRepository.EntitiesContext.Users.Add(user);

            await AppRepository.EntitiesContext.SaveChangesAsync();
        }

        public async Task UpdateTraps(string id, List<UserTrapRequest> traps)
        {
            User actualUser = await AppRepository.EntitiesContext.Users.Where(u => u.Id == new Guid(id)).Include(u => u.Traps).FirstOrDefaultAsync();

            actualUser.UpdateDate = DateTime.UtcNow;

            AppRepository.EntitiesContext.UserTraps.RemoveRange(actualUser.Traps);

            await AppRepository.EntitiesContext.SaveChangesAsync();

            actualUser.Traps = new List<UserTrap>();

            foreach (UserTrapRequest trap in traps)
            {
                actualUser.Traps.Add(Parse(trap));
            }

            await AppRepository.EntitiesContext.SaveChangesAsync();
        }

        public async Task UpdateLanguage(string email, string language)
        {
            User actualUser = await AppRepository.EntitiesContext.Users.FirstOrDefaultAsync(obj => obj.Email == email);

            actualUser.UpdateDate = DateTime.UtcNow;
            actualUser.Language = language;

            await AppRepository.EntitiesContext.SaveChangesAsync();
        }

        public async Task UpdateName(string id, string name)
        {
            User actualUser = await AppRepository.EntitiesContext.Users.FindAsync(new Guid(id));

            actualUser.UpdateDate = DateTime.UtcNow;
            actualUser.Name = name;

            await AppRepository.EntitiesContext.SaveChangesAsync();
        }

        public async Task UpdatePlatformId(string email, int platformId)
        {
            User actualUser = await AppRepository.EntitiesContext.Users.FirstOrDefaultAsync(obj => obj.Email == email);

            actualUser.UpdateDate = DateTime.UtcNow;
            actualUser.PlatformId = platformId;

            await AppRepository.EntitiesContext.SaveChangesAsync();
        }

        public async Task UpdateAppRegistration(string email, string appRegistration)
        {
            User actualUser = await AppRepository.EntitiesContext.Users.FirstOrDefaultAsync(obj => obj.Email == email);

            actualUser.UpdateDate = DateTime.UtcNow;
            actualUser.AppRegistration = appRegistration;

            await AppRepository.EntitiesContext.SaveChangesAsync();
        }

        public async Task UpdateTimeZone(string email, string timeZone)
        {
            User actualUser = await AppRepository.EntitiesContext.Users.FirstOrDefaultAsync(obj => obj.Email == email);

            actualUser.UpdateDate = DateTime.UtcNow;
            actualUser.TimeZone = timeZone;

            await AppRepository.EntitiesContext.SaveChangesAsync();
        }

        public async Task UpdateProfilePicture(string id, ImageRequest image)
        {
            User actualUser = await AppRepository.EntitiesContext.Users.FindAsync(new Guid(id));

            actualUser.UpdateDate = DateTime.UtcNow;
            actualUser.ProfilePicture = Parse(image);

            await AppRepository.EntitiesContext.SaveChangesAsync();
        }

        private UserResult Parse(User user)
        {
            if (user != null)
            {
                UserResult response = new UserResult();

                response.Id = user.Id.ToString();
                response.Name = user.Name;
                response.Points = user.Points;
                response.Email = user.Email;
                response.Language = user.Language;
                response.TimeZone = user.TimeZone;
                response.AppRegistration = user.AppRegistration;
                response.Platform = (EPlatform)user.PlatformId;
                response.RegisterType = (ERegisterType)user.RegisterType;

                if (user.ProfilePicture != null)
                {
                    response.ProfilePicture = new ImageResult();

                    response.ProfilePicture.Url = user.ProfilePicture.Url;
                    response.ProfilePicture.OriginUrl = user.ProfilePicture.OriginUrl;
                }

                if (user.Traps != null)
                {
                    response.Traps = new List<UserTrapResult>();

                    foreach (UserTrap userTrap in user.Traps)
                    {
                        response.Traps.Add(Parse(userTrap));
                    }
                }

                return response;
            }
            else
            {
                return null;
            }

        }

        private User Parse(UserRequest user)
        {
            if (user != null)
            {
                User response = new User();

                response.Name = user.Name;
                response.Points = user.Points;
                response.Email = user.Email;
                response.Language = user.Language;
                response.PlatformId = (int)user.Platform;
                response.AppRegistration = user.AppRegistration;
                response.RegisterProfileId = user.RegisterProfileId;
                response.RegisterType = (int)user.RegisterType;
                response.TimeZone = user.TimeZone;
                response.ProfilePicture = Parse(user.ProfilePicture);

                if (user.Traps != null)
                {
                    response.Traps = new List<UserTrap>();

                    foreach (UserTrapRequest userTrap in user.Traps)
                    {
                        response.Traps.Add(Parse(userTrap));
                    }
                }

                return response;
            }
            else
            {
                return null;
            }
        }

        private UserTrap Parse(UserTrapRequest userTrap)
        {
            if (userTrap != null)
            {
                UserTrap response = new UserTrap();

                response.Trap = AppRepository.EntitiesContext.Traps.Find(new Guid(userTrap.TrapId));
                response.Amount = userTrap.Amount;

                return response;
            }
            else
            {
                return null;
            }
        }

        private UserTrapResult Parse(UserTrap userTrap)
        {
            if (userTrap != null)
            {
                UserTrapResult response = new UserTrapResult();

                response.TrapId = userTrap.Trap.Id.ToString();
                response.NameKey = userTrap.Trap.NameKey;
                response.Amount = userTrap.Amount;

                return response;
            }
            else
            {
                return null;
            }
        }

        private Image Parse(ImageRequest image)
        {
            if (image != null)
            {
                Image response = new Image();

                response.Url = image.Url;
                response.OriginUrl = image.OriginUrl;

                return response;
            }
            else
            {
                return null;
            }
        }
    }
}