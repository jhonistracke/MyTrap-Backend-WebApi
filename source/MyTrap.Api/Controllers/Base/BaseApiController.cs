using MyTrap.Business;
using MyTrap.Model.Enums;
using MyTrap.Model.Mobile.Result;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MyTrap.Api.Controllers.Base
{
    public class BaseApiController : ApiController
    {
        private const string HEADER_PLATFORM = "X-App-Platform";
        private const string HEADER_TIMEZONE = "X-App-TimeZone";
        private const string HEADER_APP_REGISTRATION = "X-App-Registration";

        private const string HEADER_LANGUAGE = "Accept-Language";
        private const string HEADER_AUTHORIZATION = "Authorization";

        private UserResult _user;

        public BaseApiController() : base()
        {
            if (!string.IsNullOrEmpty(Language))
            {
                try
                {
                    string[] itensLang = Language.Split(',');

                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(itensLang[0]);
                }
                catch
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                }
            }
        }

        public static string GetAuthorizationToken(HttpRequestHeaders headers)
        {
            return headers.GetValues(HEADER_AUTHORIZATION).FirstOrDefault();
        }

        public async Task<UserResult> GetUser()
        {
            if (_user == null)
            {
                _user = await AppBusiness.User.GetByTokenAsync(TokenRequest);
            }

            return _user;
        }

        private string TokenRequest
        {
            get
            {
                return HttpContext.Current.Request.Headers[HEADER_AUTHORIZATION] ?? "";
            }
        }

        public string Language
        {
            get
            {
                string lang = HttpContext.Current.Request.Headers[HEADER_LANGUAGE] ?? "";

                return lang.Replace("_", "-");
            }
        }

        public string TimeZone
        {
            get
            {
                return HttpContext.Current.Request.Headers[HEADER_TIMEZONE] ?? "";
            }
        }

        public string AppRegistration
        {
            get
            {
                return HttpContext.Current.Request.Headers[HEADER_APP_REGISTRATION] ?? "";
            }
        }

        public EPlatform Platform
        {
            get
            {
                string platformValue = HttpContext.Current.Request.Headers[HEADER_PLATFORM] ?? "";

                if (!string.IsNullOrEmpty(platformValue))
                {
                    return (EPlatform)Convert.ToInt32(platformValue);
                }
                else
                {
                    return EPlatform.UNDEFINED;
                }
            }
        }
    }
}