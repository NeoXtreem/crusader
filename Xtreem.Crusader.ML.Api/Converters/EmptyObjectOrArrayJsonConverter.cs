using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace Xtreem.Crusader.ML.Api.Converters
{
    internal class EmptyObjectOrArrayJsonConverter<T> : JsonConverter<ICollection<T>> where T : class, new()
    {
        public override void WriteJson(JsonWriter writer, ICollection<T> value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.Any() ? (object)value : new { });
        }

        public override ICollection<T> ReadJson(JsonReader reader, Type objectType, ICollection<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return reader.TokenType switch
            {
                JsonToken.StartObject => new Collection<T>(),
                JsonToken.StartArray => serializer.Deserialize<ICollection<T>>(reader),
                _ => throw new ArgumentOutOfRangeException($"Converter does not support JSON token type {reader.TokenType}.")
            };
        }
    }
}
