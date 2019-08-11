using System.Threading.Tasks;
using Xtreem.CryptoPrediction.Data.Repositories.Interfaces;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Api.Repositories.Interfaces
{
    public interface IMarketDataReadViewRepository : IMarketDataReadRepository
    {
        Task<long> GetNextTimeAsync(string baseCurrency, string quoteCurrency, Resolution resolution, long from);
    }
}
