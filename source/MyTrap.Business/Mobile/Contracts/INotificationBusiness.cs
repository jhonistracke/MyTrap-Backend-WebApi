using MyTrap.Framework.Base;
using System.Threading.Tasks;

namespace MyTrap.Business.Mobile.Contracts
{
    public interface INotificationBusiness : IBaseBusiness
    {
        Task Register(string email, int platformId, string pushRegistrationId);

        Task SendNotificationTrapDisarmed(string email, string msg, bool owner, int points, string trapNameKey, double latitude, double longitude, string otherUserName, string otherUserImage);
    }
}