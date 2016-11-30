using Newtonsoft.Json;

namespace MyTrap.Model.Cache
{
    public class UserTokenCache
    {
        public string UserId { get; set; }
        public string Token { get; set; }

        [JsonIgnore]
        public string CacheKey
        {
            get
            {
                if (!string.IsNullOrEmpty(Token))
                {
                    return string.Format("mytrap_user_token_{0}", Token);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}