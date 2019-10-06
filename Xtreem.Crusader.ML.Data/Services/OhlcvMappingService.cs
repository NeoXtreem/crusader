using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Services.Abstractions;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.ML.Data.Services.Interfaces;

namespace Xtreem.Crusader.ML.Data.Services
{
    public class OhlcvMappingService : MappingService<OhlcvInput, Ohlcv>, IOhlcvMappingService
    {
        public OhlcvMappingService() => InitialiseMapper();
    }
}