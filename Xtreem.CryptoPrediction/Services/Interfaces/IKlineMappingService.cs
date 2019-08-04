using Binance.Net.Objects;
using Xtreem.CryptoPrediction.Service.Models;

namespace Xtreem.CryptoPrediction.Service.Services.Interfaces
{
    public interface IKlineMappingService : IMappingService<BinanceKline, InputKline>
    {
    }
}
