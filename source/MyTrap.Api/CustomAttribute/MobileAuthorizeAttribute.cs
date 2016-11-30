using MyTrap.Api.Controllers.Base;
using MyTrap.Business;
using MyTrap.Model.ViewModel.Base;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace MyTrap.Api.CustomAttribute
{
    public class MobileAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            string authorizationToken = BaseApiController.GetAuthorizationToken(actionContext.Request.Headers);

            bool authorized = AppBusiness.Security.IsTokenActive(authorizationToken);

            if (!authorized)
            {
                BaseApiResult response = new BaseApiResult();

                response.Error = true;
                response.Code = 401;
                response.Message = "Authorization token invalid.";

                string json = JsonConvert.SerializeObject(response);

                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
            }
        }
    }
}