using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xtreem.Crusader.Data.Models;

namespace Xtreem.Crusader.ML.Api.Services.Interfaces
{
    internal interface ICryptoCompareService
    {
        Task<IEnumerable<Ohlcv>> LoadHistoricalDataAsync(CurrencyPairChartPeriod currencyPairChartPeriod, CancellationToken cancellationToken);
    }
}