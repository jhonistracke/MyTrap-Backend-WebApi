using AutoMapper;
using MyTrap.Api.Controllers.Base;
using MyTrap.Api.CustomAttribute;
using MyTrap.Business;
using MyTrap.Model.Mobile.Request;
using MyTrap.Model.ViewModel.Base;
using MyTrap.Model.ViewModel.Request;
using System.Threading.Tasks;
using System.Web.Http;

namespace MyTrap.Api.Controllers
{
    public class PositionController : BaseApiController
    {
        [HttpPost]
        [MobileAuthorize]
        [ApiExceptionHandler]
        public async Task<IHttpActionResult> Send(PositionApiRequest request)
        {
            PositionRequest positionRequest = Mapper.Map<PositionRequest>(request);

            positionRequest.UserId = (await GetUser()).Id;

            await AppBusiness.Trap.AddPositionProcessQueue(positionRequest);

            return Ok(new BaseApiResult());
        }
    }
}