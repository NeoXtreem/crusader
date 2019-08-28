using Newtonsoft.Json;

namespace Xtreem.Crusader.Client.Models
{
    public class StatusResponse
    {
        [JsonProperty("s", Required = Required.Always)]
        public string S { get; protected set; } = "ok";
    }
}
