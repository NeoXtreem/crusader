using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Xtreem.CryptoPrediction.Api.Repositories.Interfaces;
using Xtreem.CryptoPrediction.Data.Contexts.Interfaces;
using Xtreem.CryptoPrediction.Data.Models;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Api.Repositories
{
    internal class MarketDataRepository : IMarketDataRepository
    {
        private readonly IMarketDataContext _context;

        public MarketDataRepository(IMarketDataContext context) => _context = context;

        public async Task<IEnumerable<Ohlcv>> GetOhlcvsAsync(string baseCurrency, string quoteCurrency, Resolution resolution, long from, long to)
        {
            using (var cursor = await _context.HistoricalOhlcvCollection.FindAsync(o => o.Base == baseCurrency && o.Quote == quoteCurrency && o.Resolution == resolution && o.Time >= from && o.Time <= to))
            {
                while (await cursor.MoveNextAsync())
                {
                    /*TODO: yield in C# 8.0*/ return cursor.Current;
                }
            }

            //TODO: yield break; in C# 8.0
            return Enumerable.Empty<Ohlcv>();
        }

        public async Task<long> GetNextTimeAsync(string baseCurrency, string quoteCurrency, Resolution resolution, long from)
        {
            using (var cursor = await _context.HistoricalOhlcvCollection.FindAsync(o => o.Base == baseCurrency && o.Quote == quoteCurrency && o.Resolution == resolution && o.Time < from))
            {
                while (await cursor.MoveNextAsync())
                {
                    if (cursor.Current.Any())
                    {
                        return cursor.Current.Max(o => o.Time);
                    }
                }
            }

            return default;
        }
    }
}
