using Newtonsoft.Json;
using System;

namespace MyTrap.Model.ViewModel.Request
{
    public class BuyIntentApiRequest
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "availableTrapId")]
        public string AvailableTrapId { get; set; }

        [JsonProperty(PropertyName = "storeKey")]
        public string StoreKey { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }
    }
}