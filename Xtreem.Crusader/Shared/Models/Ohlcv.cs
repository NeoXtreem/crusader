using JetBrains.Annotations;

namespace Xtreem.Crusader.Shared.Models
{
    [UsedImplicitly]
    public class Ohlcv
    {
        public string Base { get; set; }

        public string Quote { get; set; }

        public string Resolution { get; set; }

        [UsedImplicitly]
        public long Time { get; set; }

        [UsedImplicitly]
        public float Open { get; set; }

        [UsedImplicitly]
        public float Close { get; set; }

        [UsedImplicitly]
        public float High { get; set; }

        [UsedImplicitly]
        public float Low { get; set; }

        [UsedImplicitly]
        public float VolumeFrom { get; set; }

        [UsedImplicitly]
        public float VolumeTo { get; set; }
    }
}
