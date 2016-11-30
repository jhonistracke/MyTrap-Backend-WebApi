using MyTrap.Framework.Base;
using MyTrap.Model.Mobile.Request;
using MyTrap.Model.Mobile.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrap.Business.Mobile.Contracts
{
    public interface ITrapBusiness : IBaseBusiness
    {
        Task<TrapResult> GetById(string trapId);

        Task<TrapResult> GetByNameKey(string nameKey);

        Task<UserResult> ArmTrap(ArmedTrapRequest request);

        Task AddPositionProcessQueue(PositionRequest request);

        Task ProcessDisarmedTrap(string userId, string disarmedTrapId);

        Task<List<ArmedTrapResult>> ListArmedTraps(UserRequest request);
    }
}