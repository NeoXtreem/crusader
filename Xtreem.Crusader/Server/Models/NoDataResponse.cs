﻿using Newtonsoft.Json;

namespace Xtreem.Crusader.Server.Models
{
    internal class NoDataResponse : StatusResponse
    {
        public NoDataResponse() => S = "no_data";

        [JsonProperty("nextTime", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long NextTime { get; set; }
    }
}
