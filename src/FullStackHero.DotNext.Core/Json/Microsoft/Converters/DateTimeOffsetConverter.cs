namespace FullStackHero.DotNext.Core.Json.Microsoft.Converters;

public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    #region Overrides of JsonConverter<DateTimeOffset>

    /// <summary>Reads and converts the JSON to type DateTimeOffset.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null: return default;

            case JsonTokenType.String:
                // try to parse number directly from bytes
                var source = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;

                if (Utf8Parser.TryParse(source, out DateTimeOffset value, out var bytesConsumed) && source.Length == bytesConsumed) return value;

                // try to parse from a string if the above failed, this covers cases with other escaped/UTF characters
                return DateTimeOffset.TryParse(reader.GetString(), out var result) ? result : default;

            default:
                // fallback to default handling
                return reader.GetDateTimeOffset();
        }
    }

    /// <summary>Writes a specified value as JSON.</summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString("O"));

    #endregion
}