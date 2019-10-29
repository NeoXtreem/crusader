using System;

namespace Xtreem.Crusader.ML.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class LabelColumnAttribute : Attribute
    {
        public string ScoreColumnName { get; }

        public LabelColumnAttribute(string scoreColumnName)
        {
            ScoreColumnName = scoreColumnName;
        }
    }
}
