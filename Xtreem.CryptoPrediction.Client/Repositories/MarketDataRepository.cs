using System.Collections.Generic;
using System.Threading.Tasks;
using Xtreem.CryptoPrediction.Data.Contexts.Interfaces;
using Xtreem.CryptoPrediction.Data.Models;
using Xtreem.CryptoPrediction.Data.Repositories.Interfaces;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Client.Repositories
{
    public class MarketDataRepository : IMarketDataRepository
    {
        private readonly IMarketDataContext _context;

        public MarketDataRepository(IMarketDataContext context) => _context = context;

        public async Task AddOhlcvsAsync(IEnumerable<Ohlcv> items, Resolution resolution)
        {
            await _context.HistoricalOhlcvCollection.InsertManyAsync(items);
        }
    }
}
