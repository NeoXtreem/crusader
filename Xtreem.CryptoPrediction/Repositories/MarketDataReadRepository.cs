using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Xtreem.CryptoPrediction.Data.Contexts.Interfaces;
using Xtreem.CryptoPrediction.Data.Models;
using Xtreem.CryptoPrediction.Data.Repositories.Interfaces;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Data.Repositories
{
    public class MarketDataReadRepository : IMarketDataReadRepository
    {
        private readonly IMarketDataContext _context;

        public MarketDataReadRepository(IMarketDataContext context) => _context = context;

        public async Task<IEnumerable<Ohlcv>> GetOhlcvsAsync(string baseCurrency, string quoteCurrency, Resolution resolution, long from, long to)
        {
            using (var cursor = await _context.HistoricalOhlcvCollection.FindAsync(o => o.Base == baseCurrency && o.Quote == quoteCurrency && o.Resolution == resolution.ToString() && o.Time >= from && o.Time <= to))
            {
                var ohlcvs = new List<Ohlcv>();
                while (await cursor.MoveNextAsync())
                {
                    //TODO: yield return cursor.Current; in C# 8.0
                    ohlcvs.AddRange(cursor.Current);
                }

                return ohlcvs;
            }

            //TODO: yield break; in C# 8.0
        }

        public async Task<IEnumerable<Ohlcv>> GetOhlcvsAsync(string baseCurrency, string quoteCurrency, Resolution resolution, DateTime from, DateTime to)
        {
            return await GetOhlcvsAsync(baseCurrency, quoteCurrency, resolution, ((DateTimeOffset)from).ToUnixTimeSeconds(), ((DateTimeOffset)to).ToUnixTimeSeconds());
        }
    }
}
