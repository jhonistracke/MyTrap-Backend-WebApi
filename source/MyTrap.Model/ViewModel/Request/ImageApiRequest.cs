using Newtonsoft.Json;

namespace MyTrap.Model.ViewModel.Request
{
    public class ImageApiRequest
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}