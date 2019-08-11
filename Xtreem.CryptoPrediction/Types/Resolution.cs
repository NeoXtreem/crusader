using System.ComponentModel;

namespace Xtreem.CryptoPrediction.Data.Types
{
    public enum Resolution
    {
        [Description("1")]
        Minute,

        [Description("60")]
        Hour,

        [Description("D")]
        Day
    }
}
