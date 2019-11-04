using Newtonsoft.Json;

namespace Xtreem.Crusader.Server.Models
{
    public class StatusResponse
    {
        [JsonProperty("s", Required = Required.Always)]
        public string S { get; protected set; } = "ok";
    }
}
