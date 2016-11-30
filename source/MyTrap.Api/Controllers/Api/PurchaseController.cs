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
    public class PurchaseController : BaseApiController
    {
        [HttpGet]
        [MobileAuthorize]
        [ApiExceptionHandler]
        public async Task<IHttpActionResult> AvailableTraps()
        {
            List<AvailableTrapResult> availableTraps = await AppBusiness.Purchase.ListAvailableTraps();

            List<AvailableTrapApiResult> response = new List<AvailableTrapApiResult>();

            if (availableTraps != null)
            {
                foreach (AvailableTrapResult availableTrap in availableTraps)
                {
                    AvailableTrapApiResult availableTrapResponse = Mapper.Map<AvailableTrapApiResult>(availableTrap);

                    response.Add(availableTrapResponse);
                }
            }

            BaseListApiResult<AvailableTrapApiResult> listResult = new BaseListApiResult<AvailableTrapApiResult>();

            listResult.AddRange(response);

            return Ok(listResult);
        }

        [HttpPost]
        [MobileAuthorize]
        [ApiExceptionHandler]
        public async Task<IHttpActionResult> InsertBuyIntent(BuyIntentApiRequest request)
        {
            request.UserId = (await GetUser()).Id;

            BuyIntentRequest buyIntentRequest = Mapper.Map<BuyIntentRequest>(request);

            BuyIntentResult buyIntentResult = await AppBusiness.Purchase.RegisterBuyIntent(buyIntentRequest);

            BuyIntentApiResult response = Mapper.Map<BuyIntentApiResult>(buyIntentResult);

            return Ok(response);
        }

        [HttpPost]
        [MobileAuthorize]
        [ApiExceptionHandler]
        public async Task<IHttpActionResult> RegisterPurchase(BuyIntentApiRequest request)
        {
            BuyIntentRequest buyIntentRequest = Mapper.Map<BuyIntentRequest>(request);

            UserResult userResult = await AppBusiness.Purchase.ConfirmBuyIntent(buyIntentRequest);

            UserApiResult response = Mapper.Map<UserApiResult>(userResult);

            return Ok(response);
        }

        [HttpPost]
        [MobileAuthorize]
        [ApiExceptionHandler]
        public async Task<IHttpActionResult> InvalidateBuyIntent(BuyIntentApiRequest request)
        {
            BuyIntentRequest buyIntentRequest = Mapper.Map<BuyIntentRequest>(request);

            await AppBusiness.Purchase.DenyBuyIntent(buyIntentRequest);

            return Ok(new BaseApiResult());
        }
    }
}