using JetBrains.Annotations;

namespace Xtreem.Crusader.ML.Api.Settings
{
    internal class CryptoCompareSettings
    {
        [UsedImplicitly]
        public string BaseUrl { get; set; }

        [UsedImplicitly]
        public string ApiKey { get; set; }
    }
}
