using JetBrains.Annotations;
using Xtreem.Crusader.ML.Data.Attributes;

namespace Xtreem.Crusader.ML.Data.Models
{
    public class OhlcvInput : CurrencyPairChartTime
    {
        [LabelColumn(nameof(OhlcvRegressionPrediction.OpenPrediction)), UsedImplicitly]
        public float Open { get; set; }

        [LabelColumn(nameof(OhlcvRegressionPrediction.ClosePrediction)), UsedImplicitly]
        public float Close { get; set; }

        [LabelColumn(nameof(OhlcvRegressionPrediction.HighPrediction)), UsedImplicitly]
        public float High { get; set; }

        [LabelColumn(nameof(OhlcvRegressionPrediction.LowPrediction)), UsedImplicitly]
        public float Low { get; set; }

        [LabelColumn(nameof(OhlcvRegressionPrediction.VolumeFromPrediction)), UsedImplicitly]
        public float VolumeFrom { get; set; }

        [LabelColumn(nameof(OhlcvRegressionPrediction.VolumeToPrediction)), UsedImplicitly]
        public float VolumeTo { get; set; }
    }
}
