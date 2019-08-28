using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Xtreem.Crusader.Client.Types;

namespace Xtreem.Crusader.Client.Models
{
    public class SymbolInfoResponse
    {
        [JsonProperty("name", Required = Required.Always), Required]
        public string Name { get; set; }

        [JsonProperty("description", Required = Required.Always), Required]
        public string Description { get; set; }

        [JsonProperty("exchange-listed", Required = Required.Always), Required]
        public string ExchangeListed { get; set; }

        [JsonProperty("exchange-traded", Required = Required.Always), Required]
        public string ExchangeTraded { get; set; }

        [JsonProperty("minmov", Required = Required.Always), Required]
        public int MinMov { get; set; }

        [JsonProperty("minmov2", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MinMov2 { get; set; }

        [JsonProperty("fractional", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<bool> Fractional { get; set; }

        [JsonProperty("pricescale", Required = Required.Always), Required]
        public int PriceScale { get; set; }

        [JsonProperty("has_intraday", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool HasIntraday { get; set; }

        [JsonProperty("has_no_volume", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool HasNoVolume { get; set; }

        [JsonProperty("type", Required = Required.Always), Required]
        public Type Type { get; set; }

        [JsonProperty("ticker", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Ticker { get; set; }

        [JsonProperty("timezone", Required = Required.Always), Required]
        public string Timezone { get; set; }

        [JsonProperty("session", Required = Required.Always), Required]
        public string Session { get; set; }

        [JsonProperty("supported_resolutions", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<string> SupportedResolutions { get; set; }

        [JsonProperty("force-session-rebuild", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool ForceSessionRebuild { get; set; }

        [JsonProperty("has-daily", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool HasDaily { get; set; }

        [JsonProperty("intraday-multipliers", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<string> IntradayMultipliers { get; set; }

        [JsonProperty("has-weekly-and-monthly", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool HasWeeklyAndMonthly { get; set; }

        [JsonProperty("has-empty-bars", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool HasEmptyBars { get; set; }
    }
}
