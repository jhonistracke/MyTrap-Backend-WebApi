using MyTrap.Framework.Base;
using MyTrap.Model.Mobile.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrap.Repository.Mobile.Contracts
{
    public interface IParameterRepository : IBaseRepository
    {
        Task<List<ParameterResult>> List();
    }
}