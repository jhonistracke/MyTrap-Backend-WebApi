using Newtonsoft.Json;
using System;

namespace MyTrap.Model.Mobile.Request
{
    public class PositionRequest
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTime Date { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }
    }
}