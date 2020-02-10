using JetBrains.Annotations;
using Xtreem.Crusader.ML.Data.Attributes;

namespace Xtreem.Crusader.ML.Data.Models
{
    public class OhlcvInput : CurrencyPairChartTime
    {
        [PredictColumn, UsedImplicitly]
        public float Open { get; set; }

        [PredictColumn, UsedImplicitly]
        public float Close { get; set; }

        [PredictColumn, UsedImplicitly]
        public float High { get; set; }

        [PredictColumn, UsedImplicitly]
        public float Low { get; set; }

        [PredictColumn, UsedImplicitly]
        public float VolumeFrom { get; set; }

        [PredictColumn, UsedImplicitly]
        public float VolumeTo { get; set; }
    }
}
