using MyTrap.Framework.Base;
using MyTrap.Model.Mobile.Request;
using MyTrap.Model.Mobile.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrap.Repository.Mobile.Contracts
{
    public interface IPurchaseRepository : IBaseRepository
    {
        Task ConfirmBuyIntent(BuyIntentRequest request);

        Task<BuyIntentResult> GetById(string id);

        Task<BuyIntentResult> InsertBuyIntent(BuyIntentRequest request);

        Task DenyBuyIntent(BuyIntentRequest request);

        Task<List<AvailableTrapResult>> ListAvailableTraps();

        Task<AvailableTrapResult> GetAvailableTrapById(string id);
    }
}