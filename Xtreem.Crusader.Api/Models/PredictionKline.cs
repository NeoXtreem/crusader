using Microsoft.ML.Data;

namespace Xtreem.Crusader.Api.Models
{
    internal class PredictionKline
    {
        [ColumnName("Score")]
        public float Close;
    }
}
