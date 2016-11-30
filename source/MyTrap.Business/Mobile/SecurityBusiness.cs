using MyTrap.Business.Mobile.Contracts;
using MyTrap.Model.Mobile.Result;

namespace MyTrap.Business.Mobile
{
    public class SecurityBusiness : ISecurityBusiness
    {
        public bool IsTokenActive(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                UserResult user = AppBusiness.User.GetByToken(token);

                if (user == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}