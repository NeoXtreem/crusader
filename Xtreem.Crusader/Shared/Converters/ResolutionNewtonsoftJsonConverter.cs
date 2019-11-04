using System;
using Newtonsoft.Json;
using Xtreem.Crusader.Shared.Types;

namespace Xtreem.Crusader.Shared.Converters
{
    internal class ResolutionNewtonsoftJsonConverter : JsonConverter<Resolution>
    {
        public override void WriteJson(JsonWriter writer, Resolution value, JsonSerializer serializer) => writer.WriteValue(value.ToString());

        public override Resolution ReadJson(JsonReader reader, Type objectType, Resolution existingValue, bool hasExistingValue, JsonSerializer serializer) => Resolution.Parse((string)reader.Value);
    }
}
