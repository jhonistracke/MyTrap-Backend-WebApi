using Newtonsoft.Json;
using System;

namespace MyTrap.Model.ViewModel.Request
{
    public class PositionApiRequest
    {
        [JsonProperty(PropertyName = "lat")]
        public float Latitude { get; set; }

        [JsonProperty(PropertyName = "lng")]
        public float Longitude { get; set; }

        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }
    }
}