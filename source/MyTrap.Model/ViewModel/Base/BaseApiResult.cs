﻿using Newtonsoft.Json;

namespace MyTrap.Model.ViewModel.Base
{
    public class BaseApiResult
    {
        [JsonProperty(PropertyName = "error")]
        public bool Error { get; set; }

        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}