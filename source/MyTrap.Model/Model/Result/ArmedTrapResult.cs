using System;

namespace MyTrap.Model.Mobile.Result
{
    public class ArmedTrapResult
    {
        public string Id { get; set; }
        public string NameKey { get; set; }
        public DateTime Date { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public bool Disarmed { get; set; }
    }
}