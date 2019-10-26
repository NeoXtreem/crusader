using System.Collections.ObjectModel;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.ML.Data.Models;

namespace Xtreem.Crusader.ML.Api.Services.Interfaces
{
    public interface IPredictionService
    {
        ReadOnlyCollection<OhlcvPrediction> Predict(CurrencyPairChartPeriod predictionPeriod);
    }
}
