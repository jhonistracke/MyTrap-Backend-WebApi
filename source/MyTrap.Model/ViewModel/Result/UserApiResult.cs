using MyTrap.Model.ViewModel.Base;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MyTrap.Model.ViewModel.Result
{
    public class UserApiResult : BaseApiResult
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "profilePicture")]
        public ImageApiResult ProfilePicture { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "points")]
        public int Points { get; set; }

        [JsonProperty(PropertyName = "traps")]
        public List<UserTrapApiResult> Traps { get; set; }
    }
}