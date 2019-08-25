using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Xtreem.Crusader.Api.Models.Udf
{
    public class HistoryResponse : StatusResponse
    {
        [JsonProperty("t", Required = Required.DisallowNull), Required]
        public ICollection<long> T { get; set; }

        [JsonProperty("o", Required = Required.DisallowNull), Required]
        public ICollection<float> O { get; set; }

        [JsonProperty("h", Required = Required.DisallowNull), Required]
        public ICollection<float> H { get; set; }

        [JsonProperty("l", Required = Required.DisallowNull), Required]
        public ICollection<float> L { get; set; }

        [JsonProperty("c", Required = Required.DisallowNull), Required]
        public ICollection<float> C { get; set; }

        [JsonProperty("v", Required = Required.DisallowNull), Required]
        public ICollection<float> V { get; set; }
    }
}
