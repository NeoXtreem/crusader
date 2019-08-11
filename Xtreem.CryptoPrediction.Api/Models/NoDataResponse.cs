﻿using Newtonsoft.Json;

namespace Xtreem.CryptoPrediction.Api.Models
{
    public class NoDataResponse : StatusResponse
    {
        public NoDataResponse() => S = "no_data";

        [JsonProperty("nextTime", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long NextTime { get; set; }
    }
}
