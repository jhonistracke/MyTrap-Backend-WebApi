using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using MyTrap.Business.Mobile.Contracts;
using MyTrap.Framework.Context;

namespace MyTrap.Business
{
    public class AppBusiness
    {
        public static IUserBusiness User { get { return AppContext.Get<IUserBusiness>(); } }
        public static INotificationBusiness Notification { get { return AppContext.Get<INotificationBusiness>(); } }
        public static ITrapBusiness Trap { get { return AppContext.Get<ITrapBusiness>(); } }
        public static IPurchaseBusiness Purchase { get { return AppContext.Get<IPurchaseBusiness>(); } }
        public static ISecurityBusiness Security { get { return AppContext.Get<ISecurityBusiness>(); } }
        public static IBlobBusiness Blob { get { return AppContext.Get<IBlobBusiness>(); } }
        public static IParameterBusiness Parameter { get { return AppContext.Get<IParameterBusiness>(); } }

        public static void Start()
        {
            
        }
    }
}