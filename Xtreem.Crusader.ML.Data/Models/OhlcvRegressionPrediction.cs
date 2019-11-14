using JetBrains.Annotations;

namespace Xtreem.Crusader.ML.Data.Models
{
    public class OhlcvRegressionPrediction : CurrencyPairChartTime
    {
        [UsedImplicitly]
        public float OpenPrediction { get; set; }

        [UsedImplicitly]
        public float ClosePrediction { get; set; }

        [UsedImplicitly]
        public float HighPrediction { get; set; }

        [UsedImplicitly]
        public float LowPrediction { get; set; }

        [UsedImplicitly]
        public float VolumeFromPrediction { get; set; }

        [UsedImplicitly]
        public float VolumeToPrediction { get; set; }
    }
}
