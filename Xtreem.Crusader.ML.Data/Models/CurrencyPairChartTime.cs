using JetBrains.Annotations;
using Xtreem.Crusader.ML.Data.Attributes;

namespace Xtreem.Crusader.ML.Data.Models
{
    public class CurrencyPairChartTime
    {
        [EncodedColumn, UsedImplicitly]
        public string Base { get; set; }

        [EncodedColumn, UsedImplicitly]
        public string Quote { get; set; }

        [EncodedColumn, UsedImplicitly]
        public string Resolution { get; set; }

        [EncodedColumn, UsedImplicitly]
        public long Time { get; set; }
    }
}
