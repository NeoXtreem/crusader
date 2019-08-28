using Microsoft.ML.Data;

namespace Xtreem.Crusader.Api.Models.ML
{
    public class OhlcvPrediction : OhlcvInput
    {
        [ColumnName("Score")]
        public float ClosePrediction;
    }
}
