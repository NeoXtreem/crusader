using System.Collections.ObjectModel;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.ML.Data.Models;

namespace Xtreem.Crusader.Cape.Api.Services.Interfaces
{
    public interface IPredictionService
    {
        ReadOnlyCollection<OhlcvPrediction> Predict(CurrencyPairChartPeriod predictionPeriod);
    }
}
