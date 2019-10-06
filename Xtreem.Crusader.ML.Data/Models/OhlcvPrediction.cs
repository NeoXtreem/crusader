using Microsoft.ML.Data;

namespace Xtreem.Crusader.ML.Data.Models
{
    public class OhlcvPrediction : OhlcvInput
    {
        [ColumnName("Score")]
        public float ClosePrediction;
    }
}
