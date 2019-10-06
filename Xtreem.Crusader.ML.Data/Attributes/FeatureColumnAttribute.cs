using System;

namespace Xtreem.Crusader.ML.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FeatureColumnAttribute : Attribute
    {
        public bool Encode { get; set; }

        public FeatureColumnAttribute() : this(false)
        {
        }

        public FeatureColumnAttribute(bool encode)
        {
            Encode = encode;
        }
    }
}
