using System.Globalization;
using Newtonsoft.Json;

namespace Shared.Common.Helpers;

public class DecimalJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(decimal);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            if ((string)reader.Value == string.Empty)
            {
                return 0;
            }
        }
        else if (reader.TokenType is JsonToken.Float or JsonToken.Integer)
        {
            return Convert.ToDecimal(reader.Value);
        }

        throw new JsonSerializationException("Unexpected token type: " + reader.TokenType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteRawValue(((decimal)value).ToString("F2", CultureInfo.InvariantCulture));
    }
}