using Microsoft.Practices.Unity;
using MyTrap.Framework.Context;
using MyTrap.Repository.Cache;
using MyTrap.Repository.Mobile.Contracts;

namespace MyTrap.Repository
{
    public class AppRepository
    {
        public static EntitiesContext EntitiesContext
        {
            get
            {
                if (!AppContext.ContainerInstance.IsRegistered<EntitiesContext>("MyTrapThread"))
                {
                    AppContext.ContainerInstance.RegisterType<EntitiesContext, EntitiesContext>("MyTrapThread", new PerHttpRequestLifetime());
                }

                var context = AppContext.ContainerInstance.Resolve<EntitiesContext>("MyTrapThread");

                context.Database.CommandTimeout = 300;

                return context;
            }
        }

        private static RedisCache _cache;

        public static RedisCache RedisCache
        {
            get
            {
                if (_cache == null)
                {
                    _cache = new RedisCache();
                }

                return _cache;
            }
        }

        public static IUserRepository User { get { return AppContext.Get<IUserRepository>(); } }
        public static ITrapRepository Trap { get { return AppContext.Get<ITrapRepository>(); } }
        public static IPurchaseRepository Purchase { get { return AppContext.Get<IPurchaseRepository>(); } }
        public static IParameterRepository Parameter { get { return AppContext.Get<IParameterRepository>(); } }
    }
}