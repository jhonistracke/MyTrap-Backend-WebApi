using MyTrap.Model.ViewModel.Base;
using Newtonsoft.Json;

namespace MyTrap.Model.ViewModel.Result
{
    public class BuyIntentApiResult : BaseApiResult
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}