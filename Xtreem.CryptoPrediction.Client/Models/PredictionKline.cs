using Microsoft.ML.Data;

namespace Xtreem.CryptoPrediction.Client.Models
{
    internal class PredictionKline
    {
        [ColumnName("Score")]
        public float Close;
    }
}
