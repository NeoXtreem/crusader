using Microsoft.ML.Data;

namespace Xtreem.CryptoPrediction.Service.Models
{
    internal class PredictionKline
    {
        [ColumnName("Score")]
        public float Close;
    }
}
