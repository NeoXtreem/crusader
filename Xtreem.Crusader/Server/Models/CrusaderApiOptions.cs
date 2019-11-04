using JetBrains.Annotations;

namespace Xtreem.Crusader.Server.Models
{
    internal class CrusaderApiOptions
    {
        [UsedImplicitly]
        public string CapeApiBaseUrl { get; set; }

        [UsedImplicitly]
        public string MLApiBaseUrl { get; set; }
    }
}
