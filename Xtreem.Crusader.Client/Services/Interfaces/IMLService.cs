using System.Threading;
using System.Threading.Tasks;
using Xtreem.Crusader.Data.Models;

namespace Xtreem.Crusader.Client.Services.Interfaces
{
    public interface IMLService
    {
        Task<float?> PredictAsync(CurrencyPairChartPeriod currencyPairChartPeriod, CancellationToken cancellationToken);
    }
}
