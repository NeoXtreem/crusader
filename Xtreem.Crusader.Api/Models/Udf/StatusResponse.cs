using Newtonsoft.Json;

namespace Xtreem.Crusader.Api.Models.Udf
{
    public class StatusResponse
    {
        [JsonProperty("s", Required = Required.Always)]
        public string S { get; protected set; } = "ok";
    }
}
