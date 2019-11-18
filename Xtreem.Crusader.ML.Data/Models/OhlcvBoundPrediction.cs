using JetBrains.Annotations;

namespace Xtreem.Crusader.ML.Data.Models
{
    public class OhlcvBoundPrediction : OhlcvPrediction
    {
        [UsedImplicitly]
        public float ConfidenceLowerBound { get; set; }

        [UsedImplicitly]
        public float ConfidenceUpperBound { get; set; }
    }
}
