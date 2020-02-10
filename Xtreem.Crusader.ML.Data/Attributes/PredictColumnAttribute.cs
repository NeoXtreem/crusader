using System;
using System.Runtime.CompilerServices;

namespace Xtreem.Crusader.ML.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PredictColumnAttribute : Attribute
    {
        public string PredictionColumnName { get; }

        public PredictColumnAttribute(string predictionColumnName = null, [CallerMemberName] string propertyName = null)
        {
            PredictionColumnName = predictionColumnName ?? propertyName + "Prediction";
        }
    }
}
