using Binance.Net.Objects;
using Xtreem.CryptoPrediction.Service.Models;
using Xtreem.CryptoPrediction.Service.Services.Interfaces;

namespace Xtreem.CryptoPrediction.Service.Services
{
    public class KlineMappingService : MappingServiceBase<BinanceKline, InputKline>, IKlineMappingService
    {
        public KlineMappingService() => InitialiseMapper();
    }
}
