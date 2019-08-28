using Xtreem.Crusader.Data.Repositories.Interfaces;
using Xtreem.Crusader.Data.Types;

namespace Xtreem.Crusader.Client.Repositories.Interfaces
{
    public interface IMarketDataReadViewRepository : IMarketDataReadRepository
    {
        long GetNextTime(string baseCurrency, string quoteCurrency, Resolution resolution, long from);
    }
}
