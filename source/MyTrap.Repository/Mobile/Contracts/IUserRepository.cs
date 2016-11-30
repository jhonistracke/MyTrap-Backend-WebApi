using MyTrap.Framework.Base;
using MyTrap.Model.Mobile.Request;
using MyTrap.Model.Mobile.Result;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrap.Repository.Mobile.Contracts
{
    public interface IUserRepository : IBaseRepository
    {
        Task<UserResult> GetByEmail(string email);

        Task Insert(UserRequest user);

        Task UpdateName(string id, string name);

        Task UpdateTraps(string id, List<UserTrapRequest> traps);

        Task AddPoints(string email, int points);

        Task<UserResult> GetById(string id);

        Task UpdateAppRegistration(string email, string appRegistration);

        Task UpdatePlatformId(string email, int platformId);

        Task UpdateLanguage(string email, string language);

        Task UpdateTimeZone(string email, string timeZone);

        Task UpdateProfilePicture(string id, ImageRequest image);
    }
}