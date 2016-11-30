﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace MyTrap.Model.ViewModel.Base
{
    public class BaseListApiResult<T> : List<T>
    {
        [JsonProperty(PropertyName = "error")]
        public bool Error { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}