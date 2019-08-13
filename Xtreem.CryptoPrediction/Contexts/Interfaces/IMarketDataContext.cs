using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CosmosDB.BulkExecutor;
using Xtreem.CryptoPrediction.Data.Models;

namespace Xtreem.CryptoPrediction.Data.Contexts.Interfaces
{
    public interface IMarketDataContext
    {
        IOrderedQueryable<Ohlcv> GetHistoricalOhlcvsQuery();

        Task<BulkExecutor> GetHistoricalOhlcvBulkExecutorAsync();
    }
}
