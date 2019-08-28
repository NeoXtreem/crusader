using Xtreem.Crusader.Api.Models;
using Xtreem.Crusader.Api.Services.Abstractions.Interfaces;
using Xtreem.Crusader.Data.Models;

namespace Xtreem.Crusader.Api.Services.Interfaces
{
    public interface IOhlcvMappingService : IMappingService<OhlcvInput, Ohlcv>
    {
    }
}