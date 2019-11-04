using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xtreem.Crusader.Shared.Models;

namespace Xtreem.Crusader.Server.Services.Interfaces
{
    public interface IMLService
    {
        Task<ReadOnlyCollection<Ohlcv>> PredictAsync(CurrencyPairChartPeriod currencyPairChartPeriod, CancellationToken cancellationToken);
    }
}
