using AutoMapper;
using MyTrap.Api.Controllers.Base;
using MyTrap.Api.CustomAttribute;
using MyTrap.Business;
using MyTrap.Model.Mobile.Request;
using MyTrap.Model.Mobile.Result;
using MyTrap.Model.ViewModel.Request;
using MyTrap.Model.ViewModel.Result;
using System.Threading.Tasks;
using System.Web.Http;

namespace MyTrap.Api.Controllers
{
    public class UserController : BaseApiController
    {
        [HttpGet]
        [MobileAuthorize]
        [ApiExceptionHandler]
        public async Task<IHttpActionResult> Index()
        {
            UserApiResult response = Mapper.Map<UserApiResult>(await GetUser());

            if (!string.IsNullOrEmpty(AppRegistration))
            {
                await AppBusiness.User.UpdateRegistrationId(response.Email, AppRegistration, Platform);
            }

            return Ok(response);
        }

        [HttpPost]
        [ApiExceptionHandler]
        public async Task<IHttpActionResult> Login(UserApiRequest request)
        {
            request.Language = Language;
            request.Platform = Platform;
            request.AppRegistration = AppRegistration;
            request.TimeZone = TimeZone;

            UserRequest userRequest = Mapper.Map<UserRequest>(request);

            UserResult user = await AppBusiness.User.Login(userRequest);

            UserApiResult response = Mapper.Map<UserApiResult>(user);

            return Ok(response);
        }
    }
}