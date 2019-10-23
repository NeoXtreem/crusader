﻿using System.Collections.Generic;
using Newtonsoft.Json;
using Xtreem.Crusader.Data.Models;

namespace Xtreem.Crusader.Client.Models
{
    public class HistoricalDataResponse
    {
        [JsonProperty(Required = Required.DisallowNull)]
        public string Response { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public string Message { get; set; }

        [JsonProperty(Required = Required.Always)]
        public bool HasWarning { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public bool Aggregated { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int Type { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public object RateLimit { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public ConversionType ConversionType { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public int TimeFrom { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public int TimeTo { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public ICollection<Ohlcv> Data { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public string ParamWithError { get; set; }
    }
}