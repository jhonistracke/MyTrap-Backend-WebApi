using MyTrap.Framework.Base;

namespace MyTrap.Business.Mobile.Contracts
{
    public interface ISecurityBusiness : IBaseBusiness
    {
        bool IsTokenActive(string token);
    }
}