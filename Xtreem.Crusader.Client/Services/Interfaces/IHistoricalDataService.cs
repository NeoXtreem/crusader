using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xtreem.Crusader.Data.Models;

namespace Xtreem.Crusader.Client.Services.Interfaces
{
    public interface IHistoricalDataService
    {
        Task<IEnumerable<Ohlcv>> GetHistoricalDataAsync(CurrencyPairChartPeriod currencyPairChartPeriod, CancellationToken cancellationToken);
    }
}
