using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Xtreem.CryptoPrediction.Api.Repositories.Interfaces;
using Xtreem.CryptoPrediction.Data.Contexts.Interfaces;
using Xtreem.CryptoPrediction.Data.Repositories;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Api.Repositories
{
    internal class MarketDataReadViewRepository : MarketDataReadRepository, IMarketDataReadViewRepository
    {
        private readonly IMarketDataContext _context;

        public MarketDataReadViewRepository(IMarketDataContext context) : base(context) => _context = context;

        public async Task<long> GetNextTimeAsync(string baseCurrency, string quoteCurrency, Resolution resolution, long from)
        {
            using (var cursor = await _context.HistoricalOhlcvCollection.FindAsync(o => o.Base == baseCurrency && o.Quote == quoteCurrency && o.Resolution == resolution.ToString() && o.Time < from))
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
