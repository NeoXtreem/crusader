using System.Collections.Generic;
using System.Threading.Tasks;
using Xtreem.Crusader.Data.Contexts.Interfaces;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Repositories;
using Xtreem.Crusader.ML.Api.Repositories.Interfaces;

namespace Xtreem.Crusader.ML.Api.Repositories
{
    internal class MarketDataReadWriteRepository : MarketDataReadRepository, IMarketDataReadWriteRepository
    {
        private readonly IMarketDataContext _context;

        public MarketDataReadWriteRepository(IMarketDataContext context) : base(context) => _context = context;

        public async Task AddOhlcvsAsync(IEnumerable<Ohlcv> items)
        {
            await (await _context.GetHistoricalOhlcvBulkExecutorAsync()).BulkImportAsync(items, disableAutomaticIdGeneration: false);
        }
    }
}
