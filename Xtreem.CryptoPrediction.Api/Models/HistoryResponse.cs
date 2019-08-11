using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Xtreem.CryptoPrediction.Api.Models
{
    public class HistoryResponse : StatusResponse
    {
        [JsonProperty("t", Required = Required.DisallowNull), Required]
        public ICollection<long> T { get; set; }

        [JsonProperty("o", Required = Required.DisallowNull), Required]
        public ICollection<decimal> O { get; set; }

        [JsonProperty("h", Required = Required.DisallowNull), Required]
        public ICollection<decimal> H { get; set; }

        [JsonProperty("l", Required = Required.DisallowNull), Required]
        public ICollection<decimal> L { get; set; }

        [JsonProperty("c", Required = Required.DisallowNull), Required]
        public ICollection<decimal> C { get; set; }

        [JsonProperty("v", Required = Required.DisallowNull), Required]
        public ICollection<decimal> V { get; set; }
    }
}
