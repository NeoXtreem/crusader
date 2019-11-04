using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xtreem.Crusader.Shared.Models;

namespace Xtreem.Crusader.ML.Api.Services.Interfaces
{
    internal interface IHistoricalDataService
    {
        Task<IEnumerable<Ohlcv>> GetHistoricalDataAsync(CurrencyPairChartPeriod currencyPairChartPeriod, CancellationToken cancellationToken);
    }
}
