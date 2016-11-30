using MyTrap.Framework.Base;
using MyTrap.Model.Mobile.Request;
using MyTrap.Model.Mobile.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrap.Repository.Mobile.Contracts
{
    public interface ITrapRepository : IBaseRepository
    {
        Task<TrapResult> GetById(string trapId);

        Task<TrapResult> GetByNameKey(string nameKey);

        Task InsertArmedTrap(ArmedTrapRequest request);

        Task<List<ArmedTrapResult>> ListArmedTraps(float latitude, float longitude, string userId);

        Task<List<ArmedTrapResult>> ListArmedTrapsByUser(string id);

        Task<ArmedTrapResult> GetArmedTrapById(string disarmedTrapId);
    }
}