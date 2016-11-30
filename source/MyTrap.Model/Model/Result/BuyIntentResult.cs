using System;

namespace MyTrap.Model.Mobile.Result
{
    public class BuyIntentResult
    {
        public string Id { get; set; }
        public DateTime DateIntent { get; set; }
        public DateTime? DateResult { get; set; }
        public string UserId { get; set; }
        public string AvailableTrapId { get; set; }
        public string StoreKey { get; set; }
        public bool Realized { get; set; }
        public bool Processed { get; set; }
    }
}