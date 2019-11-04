using Xtreem.Crusader.Data.Repositories.Interfaces;
using Xtreem.Crusader.Shared.Types;

namespace Xtreem.Crusader.Server.Repositories.Interfaces
{
    public interface IMarketDataReadViewRepository : IMarketDataReadRepository
    {
        long GetNextTime(string baseCurrency, string quoteCurrency, Resolution resolution, long from);
    }
}
