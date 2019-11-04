using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xtreem.Crusader.Shared.Types;

namespace Xtreem.Crusader.Shared.Converters
{
    internal class ResolutionJsonConverter : JsonConverter<Resolution>
    {
        public override void Write(Utf8JsonWriter writer, Resolution value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());

        public override Resolution Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => Resolution.Parse(reader.GetString());
    }
}
