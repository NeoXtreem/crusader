using JetBrains.Annotations;
using Microsoft.ML.Data;
using Xtreem.Crusader.Api.Attributes;

namespace Xtreem.Crusader.Api.Models
{
    public class OhlcvInput
    {
        [LoadColumn(0), UsedImplicitly]
        public string Base { get; set; }

        [LoadColumn(1), UsedImplicitly]
        public string Quote { get; set; }

        [LoadColumn(2), UsedImplicitly]
        public string Resolution { get; set; }

        [LoadColumn(3), UsedImplicitly]
        public long Time { get; set; }

        [LoadColumn(4), UsedImplicitly]
        public float Open { get; set; }

        [LoadColumn(5), UsedImplicitly, LabelColumn]
        public float Close { get; set; }

        [LoadColumn(6), UsedImplicitly]
        public float High { get; set; }

        [LoadColumn(7), UsedImplicitly]
        public float Low { get; set; }

        [LoadColumn(8), UsedImplicitly]
        public float VolumeFrom { get; set; }

        [LoadColumn(9), UsedImplicitly]
        public float VolumeTo { get; set; }
    }
}
