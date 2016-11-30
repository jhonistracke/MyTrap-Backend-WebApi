using Elmah;
using System;
using System.Web;

namespace MyTrap.Framework.Utils
{
    public class ElmahUtils
    {
        public static void LogToElmah(Exception ex)
        {
            if (HttpContext.Current != null)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            else
            {
                if (httpApplication == null) InitNoContext();
                ErrorSignal.Get(httpApplication).Raise(ex);
            }
        }

        public static void LogSuccessToElmah(string message)
        {
            LogToElmah(new ElmahSuccessLog(message));
        }

        private static HttpApplication httpApplication = null;
        private static ErrorFilterConsole errorFilter = new ErrorFilterConsole();

        public static ErrorMailModule ErrorEmail = new ErrorMailModule();
        public static ErrorLogModule ErrorLog = new ErrorLogModule();
        public static ErrorTweetModule ErrorTweet = new ErrorTweetModule();

        private static void InitNoContext()
        {
            httpApplication = new HttpApplication();
            errorFilter.Init(httpApplication);

            (ErrorEmail as IHttpModule).Init(httpApplication);
            errorFilter.HookFiltering(ErrorEmail);

            (ErrorLog as IHttpModule).Init(httpApplication);
            errorFilter.HookFiltering(ErrorLog);

            (ErrorTweet as IHttpModule).Init(httpApplication);
            errorFilter.HookFiltering(ErrorTweet);
        }

        private class ErrorFilterConsole : ErrorFilterModule
        {
            public void HookFiltering(IExceptionFiltering module)
            {
                module.Filtering += new ExceptionFilterEventHandler(base.OnErrorModuleFiltering);
            }
        }
    }

    public class ElmahSuccessLog : Exception
    {
        public ElmahSuccessLog(string message)
            : base(message)
        { }
    }
}