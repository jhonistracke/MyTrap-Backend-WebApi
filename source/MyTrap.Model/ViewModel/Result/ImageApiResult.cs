using Newtonsoft.Json;

namespace MyTrap.Model.ViewModel.Result
{
    public class ImageApiResult
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}