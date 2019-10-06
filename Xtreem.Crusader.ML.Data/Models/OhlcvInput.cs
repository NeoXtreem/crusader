using JetBrains.Annotations;
using Microsoft.ML.Data;
using Xtreem.Crusader.ML.Data.Attributes;

namespace Xtreem.Crusader.ML.Data.Models
{
    public class OhlcvInput
    {
        [FeatureColumn(true), UsedImplicitly]
        public string Base { get; set; }

        [FeatureColumn(true), UsedImplicitly]
        public string Quote { get; set; }

        [FeatureColumn(true), UsedImplicitly]
        public string Resolution { get; set; }

        [FeatureColumn(true), UsedImplicitly]
        public long Time { get; set; }

        [FeatureColumn, UsedImplicitly]
        public float Open { get; set; }

        [ColumnName("Label"), UsedImplicitly]
        public float Close { get; set; }

        [FeatureColumn, UsedImplicitly]
        public float High { get; set; }

        [FeatureColumn, UsedImplicitly]
        public float Low { get; set; }

        [FeatureColumn, UsedImplicitly]
        public float VolumeFrom { get; set; }

        [FeatureColumn, UsedImplicitly]
        public float VolumeTo { get; set; }
    }
}
