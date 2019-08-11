using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Xtreem.CryptoPrediction.Common.Helpers
{
    public class EnumEx
    {
        public static T GetValueFromDescription<T>(string description) where T : struct, Enum
        {
            return (T)typeof(T).GetFields().Single(f => f.GetCustomAttribute<DescriptionAttribute>()?.Description == description).GetValue(null);
        }

        public static string GetDescriptionFromValue<T>(T value) where T : struct, System.Enum
        {
            return typeof(T).GetFields().Single(f => ((T)f.GetValue(null)).Equals(value)).GetCustomAttribute<DescriptionAttribute>().Description;
        }
    }
}
