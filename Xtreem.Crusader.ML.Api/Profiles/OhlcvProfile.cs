using AutoMapper;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.ML.Data.Models;

namespace Xtreem.Crusader.ML.Api.Profiles
{
    public class OhlcvProfile : Profile
    {
        public OhlcvProfile()
        {
            RecognizeDestinationPostfixes("Prediction");

            CreateMap<Ohlcv, OhlcvInput>();
            CreateMap<OhlcvRegressionPrediction, Ohlcv>();
            CreateMap<OhlcvTimeSeriesPrediction, Ohlcv>();
        }
    }
}
