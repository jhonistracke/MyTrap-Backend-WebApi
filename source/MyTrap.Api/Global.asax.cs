using MyTrap.Api.Handlers;
using MyTrap.Business;
using MyTrap.Framework.Context;
using MyTrap.Model.Mappers;
using MyTrap.Repository;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyTrap.Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.MessageHandlers.Add(new MessageLoggingHandler());

            AppContext.InitializeContainer(typeof(AppBusiness).Assembly, typeof(AppRepository).Assembly);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MapperConfig.Configure();

            AppBusiness.Start();
        }
    }
}