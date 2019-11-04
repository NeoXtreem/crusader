using AutoMapper;
using Xtreem.Crusader.ML.Data.Models;
using Xtreem.Crusader.Shared.Models;

namespace Xtreem.Crusader.ML.Api.Profiles
{
    public class OhlcvProfile : Profile
    {
        public OhlcvProfile()
        {
            RecognizePostfixes("Prediction");

            CreateMap<Ohlcv, OhlcvInput>();
            CreateMap<OhlcvRegressionPrediction, Ohlcv>();
            CreateMap<OhlcvTimeSeriesPrediction, Ohlcv>();
        }
    }
}
