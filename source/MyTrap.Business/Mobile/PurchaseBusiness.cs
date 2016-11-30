using AutoMapper;
using MyTrap.Business.Mobile.Contracts;
using MyTrap.Model.Mobile.Request;
using MyTrap.Model.Mobile.Result;
using MyTrap.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrap.Business.Mobile
{
    public class PurchaseBusiness : IPurchaseBusiness
    {
        public async Task<BuyIntentResult> RegisterBuyIntent(BuyIntentRequest request)
        {
            request.DateIntent = DateTime.UtcNow;

            BuyIntentResult response = await AppRepository.Purchase.InsertBuyIntent(request);

            return response;
        }

        public async Task<UserResult> ConfirmBuyIntent(BuyIntentRequest request)
        {
            UserResult response = null;

            BuyIntentResult actualIntent = await AppRepository.Purchase.GetById(request.Id);

            if (actualIntent != null)
            {
                request = Mapper.Map<BuyIntentRequest>(actualIntent);

                await AppRepository.Purchase.ConfirmBuyIntent(request);

                response = await AppBusiness.User.GetById(actualIntent.UserId);

                if (response != null)
                {
                    AvailableTrapResult availableTrap = await AppBusiness.Purchase.GetAvailableTrapById(actualIntent.AvailableTrapId);

                    if (availableTrap != null)
                    {
                        TrapResult trap = await AppBusiness.Trap.GetByNameKey(availableTrap.NameKey);

                        UserTrapRequest userTrap = new UserTrapRequest();

                        userTrap.TrapId = trap.Id.ToString();
                        userTrap.Amount = availableTrap.Amount;

                        UserRequest userRequest = Mapper.Map<UserRequest>(response);

                        AppBusiness.User.AddTrap(userRequest, userTrap);

                        await AppRepository.User.UpdateTraps(userRequest.Id, userRequest.Traps);

                        response = await AppBusiness.User.GetById(userRequest.Id);
                    }
                }
            }

            return response;
        }

        public async Task DenyBuyIntent(BuyIntentRequest request)
        {
            await AppRepository.Purchase.DenyBuyIntent(request);
        }

        public async Task<List<AvailableTrapResult>> ListAvailableTraps()
        {
            List<AvailableTrapResult> availableTraps = await AppRepository.Purchase.ListAvailableTraps();

            return availableTraps;
        }

        public async Task<AvailableTrapResult> GetAvailableTrapById(string availableTrapId)
        {
            return await AppRepository.Purchase.GetAvailableTrapById(availableTrapId);
        }
    }
}