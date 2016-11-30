using MyTrap.Framework.Utils;
using MyTrap.Model.Framework;
using MyTrap.Model.ViewModel.Base;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;

namespace MyTrap.Api.CustomAttribute
{
    public class ApiExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception is MyTrapBusinessException)
                {
                    BaseApiResult response = new BaseApiResult();

                    response.Error = true;
                    response.Code = 500;
                    response.Message = ((MyTrapBusinessException)context.Exception).Inconsistency;

                    string json = JsonConvert.SerializeObject(response);

                    context.Response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };
                }
                else
                {
                    BaseApiResult response = new BaseApiResult();

                    response.Error = true;
                    response.Code = 500;
                    response.Message = "General error";

                    string json = JsonConvert.SerializeObject(response);

                    context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };

                    ElmahUtils.LogToElmah(context.Exception);
                }
            }
        }
    }
}