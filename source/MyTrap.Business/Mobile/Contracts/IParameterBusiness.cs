using MyTrap.Framework.Base;
using System.Threading.Tasks;

namespace MyTrap.Business.Mobile.Contracts
{
    public interface IParameterBusiness : IBaseBusiness
    {
        Task<string> GetValue(string key);
    }
}