using System;

namespace MyTrap.Model.Mobile.Request
{
    public class ArmedTrapRequest
    {
        public string Id { get; set; }
        public string NameKey { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
    }
}