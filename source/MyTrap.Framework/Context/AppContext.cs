using MyTrap.Framework.Base;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyTrap.Framework.Context
{
    public class AppContext
    {
        public static IUnityContainer ContainerInstance { get; set; }

        public static void InitializeContainer(Assembly businessAssembly, Assembly repositoryAssembly)
        {
            ContainerInstance = new UnityContainer();
            ContainerInstance.RegisterType<ValidatorFactory, AttributeValidatorFactory>(new ContainerControlledLifetimeManager());

            var componentsBusiness = businessAssembly.GetTypes().Where(t => t.IsClass && typeof(IBaseBusiness).IsAssignableFrom(t));
            var componentsRepository = repositoryAssembly.GetTypes().Where(t => t.IsClass && typeof(IBaseRepository).IsAssignableFrom(t));

            List<Type> components = componentsBusiness.ToList();
            components.AddRange(componentsRepository);

            foreach (var component in components)
            {
                var cpType = component.UnderlyingSystemType;

                if (cpType.GetInterfaces().Any(i => i.Name == string.Concat("I", cpType.Name)))
                {
                    ContainerInstance.RegisterType(cpType.GetInterfaces().First(), cpType, new ContainerControlledLifetimeManager());
                }
            }
        }

        public static T Get<T>()
        {
            if (ContainerInstance == null)
            {
                throw new Exception("Container instance has not been set to an IoC Container.");
            }

            return ContainerInstance.Resolve<T>(new ResolverOverride[] { });
        }

        public static T Get<T>(string name)
        {
            if (ContainerInstance == null)
            {
                throw new Exception("Container instance has not been set to an IoC Container.");
            }

            return ContainerInstance.Resolve<T>(name, new ResolverOverride[] { });
        }

        public static T GetOrRegister<T>()
        {
            if (!ContainerInstance.IsRegistered<T>())
            {
                ContainerInstance.RegisterType<T, T>(new ContainerControlledLifetimeManager());
            }

            return Get<T>();
        }

        public static T GetOrRegister<T>(string name)
        {
            if (!ContainerInstance.IsRegistered<T>(name))
            {
                ContainerInstance.RegisterType<T, T>(name, new ContainerControlledLifetimeManager());
            }

            return Get<T>(name);
        }
    }
}