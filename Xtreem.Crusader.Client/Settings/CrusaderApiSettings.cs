using JetBrains.Annotations;

namespace Xtreem.Crusader.Client.Settings
{
    internal class CrusaderApiSettings
    {
        [UsedImplicitly]
        public string CapeApiBaseUrl { get; set; }

        [UsedImplicitly]
        public string MLApiBaseUrl { get; set; }
    }
}
