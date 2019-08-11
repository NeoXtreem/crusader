using Binance.Net.Objects;
using Xtreem.CryptoPrediction.Client.Models;
using Xtreem.CryptoPrediction.Client.Services.Interfaces;

namespace Xtreem.CryptoPrediction.Client.Services
{
    public class KlineMappingService : MappingServiceBase<BinanceKline, InputKline>, IKlineMappingService
    {
        public KlineMappingService() => InitialiseMapper();
    }
}
