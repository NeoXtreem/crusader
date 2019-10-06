using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Services.Abstractions.Interfaces;
using Xtreem.Crusader.ML.Data.Models;

namespace Xtreem.Crusader.ML.Data.Services.Interfaces
{
    public interface IOhlcvMappingService : IMappingService<OhlcvInput, Ohlcv>
    {
    }
}