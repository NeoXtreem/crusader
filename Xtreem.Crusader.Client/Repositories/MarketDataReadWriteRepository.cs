using System.Collections.Generic;
using System.Threading.Tasks;
using Xtreem.Crusader.Client.Repositories.Interfaces;
using Xtreem.Crusader.Data.Contexts.Interfaces;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Repositories;
using Xtreem.Crusader.Data.Types;

namespace Xtreem.Crusader.Client.Repositories
{
    public class MarketDataReadWriteRepository : MarketDataReadRepository, IMarketDataReadWriteRepository
    {
        private readonly IMarketDataContext _context;

        public MarketDataReadWriteRepository(IMarketDataContext context) : base(context) => _context = context;

        public async Task AddOhlcvsAsync(IEnumerable<Ohlcv> items, Resolution resolution)
        {
            await (await _context.GetHistoricalOhlcvBulkExecutorAsync()).BulkImportAsync(items, disableAutomaticIdGeneration: false);
        }
    }
}
