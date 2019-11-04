using System;
using System.ComponentModel;
using System.Globalization;
using Xtreem.Crusader.Shared.Types;

namespace Xtreem.Crusader.Shared.Converters
{
    internal class ResolutionTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return value is string s ? Resolution.Parse(s) : base.ConvertFrom(context, culture, value);
        }
    }
}
