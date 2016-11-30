using MyTrap.Model.Mobile.Request;
using MyTrap.Model.Mobile.Result;
using MyTrap.Repository.Mobile.Contracts;
using MyTrap.Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MyTrap.Repository.Mobile
{
    public class PurchaseRepository : IPurchaseRepository
    {
        public async Task ConfirmBuyIntent(BuyIntentRequest request)
        {
            BuyIntent buyIntent = await AppRepository.EntitiesContext.BuyIntents.FindAsync(new Guid(request.Id));

            buyIntent.DateResult = DateTime.UtcNow;
            buyIntent.Processed = true;
            buyIntent.Realized = true;

            await AppRepository.EntitiesContext.SaveChangesAsync();
        }

        public async Task DenyBuyIntent(BuyIntentRequest request)
        {
            BuyIntent buyIntent = await AppRepository.EntitiesContext.BuyIntents.FindAsync(new Guid(request.Id));

            buyIntent.DateResult = DateTime.UtcNow;
            buyIntent.Processed = true;
            buyIntent.Realized = false;

            await AppRepository.EntitiesContext.SaveChangesAsync();
        }

        public async Task<AvailableTrapResult> GetAvailableTrapById(string id)
        {
            AvailableTrap availableTrap = await AppRepository.EntitiesContext.AvailableTraps.FindAsync(new Guid(id));

            return Parse(availableTrap);
        }

        public async Task<BuyIntentResult> GetById(string id)
        {
            BuyIntent buyIntent = await AppRepository.EntitiesContext.BuyIntents
                .Include(b => b.AvailableTrap)
                .Include(b => b.User)
                .FirstOrDefaultAsync(o => o.Id == new Guid(id));

            return Parse(buyIntent);
        }

        public async Task<BuyIntentResult> InsertBuyIntent(BuyIntentRequest request)
        {
            BuyIntent intent = Parse(request);

            AppRepository.EntitiesContext.BuyIntents.Add(intent);

            await AppRepository.EntitiesContext.SaveChangesAsync();

            return Parse(intent);
        }

        public async Task<List<AvailableTrapResult>> ListAvailableTraps()
        {
            List<AvailableTrapResult> response = new List<AvailableTrapResult>();

            List<AvailableTrap> availableTraps = await AppRepository.EntitiesContext.AvailableTraps.Where(obj => obj.Active).ToListAsync();

            if (availableTraps != null)
            {
                foreach (AvailableTrap availableTrap in availableTraps)
                {
                    response.Add(Parse(availableTrap));
                }
            }

            return response;
        }

        private AvailableTrapResult Parse(AvailableTrap availableTrap)
        {
            if (availableTrap != null)
            {
                AvailableTrapResult response = new AvailableTrapResult();

                response.Active = availableTrap.Active;
                response.Id = availableTrap.Id.ToString();
                response.KeyApple = availableTrap.KeyApple;
                response.KeyGoogle = availableTrap.KeyGoogle;
                response.KeyWindows = availableTrap.KeyWindows;
                response.NameKey = availableTrap.NameKey;
                response.Amount = availableTrap.Amount;
                response.Value = availableTrap.Value;

                return response;
            }
            else
            {
                return null;
            }
        }

        private BuyIntent Parse(BuyIntentRequest request)
        {
            if (request != null)
            {
                BuyIntent response = new BuyIntent();

                response.AvailableTrap = AppRepository.EntitiesContext.AvailableTraps.Find(new Guid(request.AvailableTrapId));
                response.DateIntent = request.DateIntent;
                response.DateResult = request.DateResult;
                response.Processed = request.Processed;
                response.Realized = request.Realized;
                response.StoreKey = request.StoreKey;
                response.User = AppRepository.EntitiesContext.Users.Find(new Guid(request.UserId));

                return response;
            }
            else
            {
                return null;
            }
        }

        private BuyIntentResult Parse(BuyIntent request)
        {
            if (request != null)
            {
                BuyIntentResult response = new BuyIntentResult();

                response.Id = request.Id.ToString();
                response.AvailableTrapId = request.AvailableTrap.Id.ToString();
                response.DateIntent = request.DateIntent;
                response.DateResult = request.DateResult;
                response.Processed = request.Processed;
                response.Realized = request.Realized;
                response.StoreKey = request.StoreKey;
                response.UserId = request.User.Id.ToString();

                return response;
            }
            else
            {
                return null;
            }
        }
    }
}