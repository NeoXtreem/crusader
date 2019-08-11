using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xtreem.CryptoPrediction.Client.Repositories.Interfaces;
using Xtreem.CryptoPrediction.Data.Contexts.Interfaces;
using Xtreem.CryptoPrediction.Data.Models;
using Xtreem.CryptoPrediction.Data.Repositories;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Client.Repositories
{
    public class MarketDataReadWriteRepository : MarketDataReadRepository, IMarketDataReadWriteRepository
    {
        private readonly IMarketDataContext _context;

        public MarketDataReadWriteRepository(IMarketDataContext context) : base(context) => _context = context;

        public async Task AddOhlcvsAsync(IEnumerable<Ohlcv> items, Resolution resolution)
        {
            var enumerable = items as Ohlcv[] ?? items.ToArray();
            if (enumerable.Any())
            {
                await _context.HistoricalOhlcvCollection.InsertManyAsync(enumerable);
            }
        }
    }
}
