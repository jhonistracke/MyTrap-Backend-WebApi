using MyTrap.Model.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MyTrap.Model.Mobile.Result
{
    public class UserResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public int Points { get; set; }
        public string TimeZone { get; set; }
        public string AppRegistration { get; set; }
        public string Language { get; set; }
        public EPlatform Platform { get; set; }
        public ERegisterType RegisterType { get; set; }
        public ImageResult ProfilePicture { get; set; }
        public List<UserTrapResult> Traps { get; set; }

        [JsonIgnore]
        public string CacheKey
        {
            get
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    return string.Format("mytrap_user_{0}", Id.ToLower());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}