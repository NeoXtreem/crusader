using System.Linq;
using JetBrains.Annotations;
using Xtreem.Crusader.Client.Repositories.Interfaces;
using Xtreem.Crusader.Data.Contexts.Interfaces;
using Xtreem.Crusader.Data.Repositories;
using Xtreem.Crusader.Data.Types;
using Xtreem.Crusader.Utilities.Attributes;

namespace Xtreem.Crusader.Client.Repositories
{
    [Inject, UsedImplicitly]
    internal class MarketDataReadViewRepository : MarketDataReadRepository, IMarketDataReadViewRepository
    {
        private readonly IMarketDataContext _context;

        public MarketDataReadViewRepository(IMarketDataContext context)
            : base(context)
        {
            _context = context;
        }

        public long GetNextTime(string baseCurrency, string quoteCurrency, Resolution resolution, long from)
        {
            return _context.GetHistoricalOhlcvsQuery().Where(o => o.Base == baseCurrency && o.Quote == quoteCurrency && o.Resolution == resolution.ToString() && o.Time < from).Max(o => o.Time);
        }
    }
}
