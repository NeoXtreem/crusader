using System.Linq;
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

        public long GetNextTime(string baseCurrency, string quoteCurrency, Resolution resolution, long from)
        {
            return _context.GetHistoricalOhlcvsQuery().Where(o => o.Base == baseCurrency && o.Quote == quoteCurrency && o.Resolution == resolution.ToString() && o.Time < from).Max(o => o.Time);
        }
    }
}
