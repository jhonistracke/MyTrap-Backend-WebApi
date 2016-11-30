using MyTrap.Framework.Base;
using MyTrap.Model.Mobile.Request;
using MyTrap.Model.Mobile.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrap.Business.Mobile.Contracts
{
    public interface IPurchaseBusiness : IBaseBusiness
    {
        Task<BuyIntentResult> RegisterBuyIntent(BuyIntentRequest request);

        Task<UserResult> ConfirmBuyIntent(BuyIntentRequest request);

        Task DenyBuyIntent(BuyIntentRequest request);

        Task<List<AvailableTrapResult>> ListAvailableTraps();

        Task<AvailableTrapResult> GetAvailableTrapById(string availableTrapId);
    }
}