using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xtreem.Crusader.Data.Models;

namespace Xtreem.Crusader.ML.Api.Services.Interfaces
{
    public interface ICryptoCompareService
    {
        Task<IEnumerable<Ohlcv>> LoadHistoricalDataAsync(CurrencyPairChartPeriod currencyPairChartPeriod, CancellationToken cancellationToken);
    }
}