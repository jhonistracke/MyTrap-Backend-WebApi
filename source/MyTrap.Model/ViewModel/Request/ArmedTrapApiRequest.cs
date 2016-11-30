using Newtonsoft.Json;

namespace MyTrap.Model.ViewModel.Request
{
    public class ArmedTrapApiRequest
    {
        [JsonProperty(PropertyName = "nameKey")]
        public string NameKey { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "lng")]
        public double Longitude { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }
    }
}