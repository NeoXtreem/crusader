using JetBrains.Annotations;
using Xtreem.Crusader.ML.Data.Attributes;

namespace Xtreem.Crusader.ML.Data.Models
{
    public class OhlcvInput
    {
        [EncodedColumn, UsedImplicitly]
        public string Base { get; set; }

        [EncodedColumn, UsedImplicitly]
        public string Quote { get; set; }

        [EncodedColumn, UsedImplicitly]
        public string Resolution { get; set; }

        [EncodedColumn, UsedImplicitly]
        public long Time { get; set; }

        [LabelColumn(nameof(OhlcvPrediction.OpenPrediction)), UsedImplicitly]
        public float Open { get; set; }

        [LabelColumn(nameof(OhlcvPrediction.ClosePrediction)), UsedImplicitly]
        public float Close { get; set; }

        [LabelColumn(nameof(OhlcvPrediction.HighPrediction)), UsedImplicitly]
        public float High { get; set; }

        [LabelColumn(nameof(OhlcvPrediction.LowPrediction)), UsedImplicitly]
        public float Low { get; set; }

        [LabelColumn(nameof(OhlcvPrediction.VolumeFromPrediction)), UsedImplicitly]
        public float VolumeFrom { get; set; }

        [LabelColumn(nameof(OhlcvPrediction.VolumeToPrediction)), UsedImplicitly]
        public float VolumeTo { get; set; }
    }
}
