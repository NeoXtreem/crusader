using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Ohlcv> GetOhlcvs(string baseCurrency, string quoteCurrency, Resolution resolution, long from, long to)
        {
            return _context.GetHistoricalOhlcvsQuery().Where(o => o.Base == baseCurrency && o.Quote == quoteCurrency && o.Resolution == resolution.ToString() && o.Time >= from && o.Time <= to).AsEnumerable();
        }

        public IEnumerable<Ohlcv> GetOhlcvs(string baseCurrency, string quoteCurrency, Resolution resolution, DateTime from, DateTime to)
        {
            return GetOhlcvs(baseCurrency, quoteCurrency, resolution, ((DateTimeOffset)from).ToUnixTimeSeconds(), ((DateTimeOffset)to).ToUnixTimeSeconds());
        }
    }
}
