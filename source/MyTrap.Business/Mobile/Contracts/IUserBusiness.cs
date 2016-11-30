using MyTrap.Framework.Base;
using MyTrap.Model.Enums;
using MyTrap.Model.Mobile.Request;
using MyTrap.Model.Mobile.Result;
using System.Threading.Tasks;

namespace MyTrap.Business.Mobile.Contracts
{
    public interface IUserBusiness : IBaseBusiness
    {
        Task<UserResult> Save(UserRequest request);

        Task<bool> UpdateUserCache(string email, string token = "");

        Task AddPoints(UserRequest user, int points);

        Task<UserResult> Login(UserRequest request);

        void AddTrap(UserRequest request, UserTrapRequest userTrap);

        Task RemoveTrap(UserRequest user, string nameKey);

        Task<UserResult> GetById(string id);

        Task<UserResult> GetByTokenAsync(string token);

        UserResult GetByToken(string token);
        Task UpdateRegistrationId(string email, string appRegistrationId, EPlatform platform);
    }
}