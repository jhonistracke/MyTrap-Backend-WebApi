using Newtonsoft.Json;

namespace MyTrap.Model.ViewModel.Result
{
    public class UserTrapApiResult
    {
        [JsonProperty(PropertyName = "nameKey")]
        public string NameKey { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }
    }
}