using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace Xtreem.Crusader.ML.Api.Converters
{
    internal class EmptyObjectOrArrayJsonConverter<TValue> : JsonConverter<ICollection<TValue>>
    {
        public override void WriteJson(JsonWriter writer, ICollection<TValue> value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.Any() ? (object)value : new { });
        }

        public override ICollection<TValue> ReadJson(JsonReader reader, Type objectType, ICollection<TValue> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return reader.TokenType switch
            {
                JsonToken.StartObject => new Collection<TValue>(),
                JsonToken.StartArray => serializer.Deserialize<ICollection<TValue>>(reader),
                _ => throw new ArgumentOutOfRangeException($"Converter does not support JSON token type {reader.TokenType}.")
            };
        }
    }
}
