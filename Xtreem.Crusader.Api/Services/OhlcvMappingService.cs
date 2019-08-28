using Xtreem.Crusader.Api.Models;
using Xtreem.Crusader.Api.Services.Abstractions;
using Xtreem.Crusader.Api.Services.Interfaces;
using Xtreem.Crusader.Data.Models;

namespace Xtreem.Crusader.Api.Services
{
    public class OhlcvMappingService : MappingService<OhlcvInput, Ohlcv>, IOhlcvMappingService
    {
        public OhlcvMappingService() => InitialiseMapper();
    }
}