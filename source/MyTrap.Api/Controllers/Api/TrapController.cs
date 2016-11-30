using AutoMapper;
using MyTrap.Api.Controllers.Base;
using MyTrap.Api.CustomAttribute;
using MyTrap.Business;
using MyTrap.Model.Mobile.Request;
using MyTrap.Model.Mobile.Result;
using MyTrap.Model.ViewModel.Base;
using MyTrap.Model.ViewModel.Request;
using MyTrap.Model.ViewModel.Result;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace MyTrap.Api.Controllers
{
    public class TrapController : BaseApiController
    {
        [HttpPost]
        [MobileAuthorize]
        [ApiExceptionHandler]
        public async Task<IHttpActionResult> Arm(ArmedTrapApiRequest request)
        {
            request.UserId = (await GetUser()).Id;

            ArmedTrapRequest armedTrapRequest = Mapper.Map<ArmedTrapRequest>(request);

            UserResult user = await AppBusiness.Trap.ArmTrap(armedTrapRequest);

            UserApiResult response = Mapper.Map<UserApiResult>(user);

            return Ok(response);
        }

        [HttpGet]
        [MobileAuthorize]
        [ApiExceptionHandler]
        public async Task<IHttpActionResult> Armed()
        {
            List<ArmedTrapResult> armedTraps = await AppBusiness.Trap.ListArmedTraps(new UserRequest() { Id = (await GetUser()).Id });

            List<ArmedTrapApiResult> response = new List<ArmedTrapApiResult>();

            if (armedTraps != null)
            {
                foreach (ArmedTrapResult armedTrap in armedTraps)
                {
                    ArmedTrapApiResult armedTrapResponse = Mapper.Map<ArmedTrapApiResult>(armedTrap);

                    response.Add(armedTrapResponse);
                }
            }

            BaseListApiResult<ArmedTrapApiResult> listResult = new BaseListApiResult<ArmedTrapApiResult>();

            listResult.AddRange(response);

            return Ok(listResult);
        }

        [HttpGet]
        [ApiExceptionHandler]
        public async Task<IHttpActionResult> ProcessDisarmedTrap(string userId, string armedTrapId)
        {
            await AppBusiness.Trap.ProcessDisarmedTrap(userId, armedTrapId);

            return Json(new BaseApiResult() { Message = "SUCCESS" });
        }
    }
}