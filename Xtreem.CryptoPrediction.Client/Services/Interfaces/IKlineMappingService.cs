using Binance.Net.Objects;
using Xtreem.CryptoPrediction.Client.Models;

namespace Xtreem.CryptoPrediction.Client.Services.Interfaces
{
    public interface IKlineMappingService : IMappingService<BinanceKline, InputKline>
    {
    }
}
