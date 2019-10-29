using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Xtreem.Crusader.Data.Contexts.Interfaces;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Repositories;
using Xtreem.Crusader.ML.Api.Repositories.Interfaces;
using Xtreem.Crusader.Utilities.Attributes;

namespace Xtreem.Crusader.ML.Api.Repositories
{
    [Inject, UsedImplicitly]
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
