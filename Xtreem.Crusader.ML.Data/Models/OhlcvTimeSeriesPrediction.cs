using JetBrains.Annotations;
using Xtreem.Crusader.ML.Data.Attributes;
using Xtreem.Crusader.ML.Data.Types;

namespace Xtreem.Crusader.ML.Data.Models
{
    [PredictionModel(PredictionModel.TimeSeries)]
    public class OhlcvTimeSeriesPrediction : CurrencyPairChartTime
    {
        [UsedImplicitly]
        public float[] OpenPrediction;

        [UsedImplicitly]
        public float[] ClosePrediction;

        [UsedImplicitly]
        public float[] HighPrediction;

        [UsedImplicitly]
        public float[] LowPrediction;

        [UsedImplicitly]
        public float[] VolumeFromPrediction;

        [UsedImplicitly]
        public float[] VolumeToPrediction;

        [UsedImplicitly]
        public float[] ConfidenceLowerBound { get; set; }

        [UsedImplicitly]
        public float[] ConfidenceUpperBound { get; set; }
    }
}
