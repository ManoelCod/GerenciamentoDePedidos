using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class CustomDecimalConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();
        
        if (decimal.TryParse(stringValue, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, new CultureInfo("pt-BR"), out var value))
        {
            return value;
        }
        
        if (decimal.TryParse(stringValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value))
        {
            return value;
        }

        throw new JsonException($"Unable to convert \"{stringValue}\" to {typeToConvert}");
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
    }
}
