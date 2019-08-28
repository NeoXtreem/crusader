using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Xtreem.Crusader.Client.Types
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Type
    {
        [EnumMember(Value = "stock")]
        Stock,

        [EnumMember(Value = "fund")]
        Fund,

        [EnumMember(Value = "dr")]
        Dr,

        [EnumMember(Value = "right")]
        Right,

        [EnumMember(Value = "bond")]
        Bond,

        [EnumMember(Value = "warrant")]
        Warrant,

        [EnumMember(Value = "structured")]
        Structured,

        [EnumMember(Value = "index")]
        Index,

        [EnumMember(Value = "forex")]
        Forex,

        [EnumMember(Value = "futures")]
        Futures,

        [EnumMember(Value = "crypto")]
        Crypto
    }
}
