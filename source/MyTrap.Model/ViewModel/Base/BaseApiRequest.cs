using MyTrap.Model.Enums;
using Newtonsoft.Json;

namespace MyTrap.Model.ViewModel.Base
{
    public class BaseApiRequest
    {
        [JsonIgnore]
        public string AppRegistration { get; set; }

        [JsonIgnore]
        public EPlatform Platform { get; set; }

        [JsonIgnore]
        public string Language { get; set; }

        [JsonIgnore]
        public string TimeZone { get; set; }
    }
}