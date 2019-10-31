using System;
using Xtreem.Crusader.ML.Data.Types;

namespace Xtreem.Crusader.ML.Data.Attributes
{
    public class PredictionModelAttribute : Attribute
    {
        public PredictionModel PredictionModel { get; }

        public PredictionModelAttribute(PredictionModel predictionModel)
        {
            PredictionModel = predictionModel;
        }
    }
}
