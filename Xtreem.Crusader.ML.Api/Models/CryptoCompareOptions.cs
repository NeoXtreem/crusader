using JetBrains.Annotations;

namespace Xtreem.Crusader.ML.Api.Models
{
    internal class CryptoCompareOptions
    {
        [UsedImplicitly]
        public string BaseUrl { get; set; }

        [UsedImplicitly]
        public string ApiKey { get; set; }
    }
}
