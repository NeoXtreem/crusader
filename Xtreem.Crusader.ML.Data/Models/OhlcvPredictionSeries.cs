using JetBrains.Annotations;

namespace Xtreem.Crusader.ML.Data.Models
{
    public class OhlcvPredictionSeries
    {
        [UsedImplicitly]
        public float[] OpenPrediction { get; set; }

        [UsedImplicitly]
        public float[] ClosePrediction { get; set; }

        [UsedImplicitly]
        public float[] HighPrediction { get; set; }

        [UsedImplicitly]
        public float[] LowPrediction { get; set; }

        [UsedImplicitly]
        public float[] VolumeFromPrediction { get; set; }

        [UsedImplicitly]
        public float[] VolumeToPrediction { get; set; }

        [UsedImplicitly]
        public float[] OpenConfidenceLowerBound { get; set; }

        [UsedImplicitly]
        public float[] OpenConfidenceUpperBound { get; set; }

        [UsedImplicitly]
        public float[] CloseConfidenceLowerBound { get; set; }

        [UsedImplicitly]
        public float[] CloseConfidenceUpperBound { get; set; }

        [UsedImplicitly]
        public float[] HighConfidenceLowerBound { get; set; }

        [UsedImplicitly]
        public float[] HighConfidenceUpperBound { get; set; }

        [UsedImplicitly]
        public float[] LowConfidenceLowerBound { get; set; }

        [UsedImplicitly]
        public float[] LowConfidenceUpperBound { get; set; }

        [UsedImplicitly]
        public float[] VolumeFromConfidenceLowerBound { get; set; }

        [UsedImplicitly]
        public float[] VolumeFromConfidenceUpperBound { get; set; }

        [UsedImplicitly]
        public float[] VolumeToConfidenceLowerBound { get; set; }

        [UsedImplicitly]
        public float[] VolumeToConfidenceUpperBound { get; set; }
    }
}
