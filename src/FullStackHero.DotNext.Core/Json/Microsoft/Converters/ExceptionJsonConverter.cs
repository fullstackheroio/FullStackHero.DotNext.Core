namespace FullStackHero.DotNext.Core.Json.Microsoft.Converters;

public class ExceptionJsonConverter : JsonConverter<Exception>
{
    #region Overrides of JsonConverter<Exception>

    /// <summary>Reads and converts the JSON to type <typeparamref name="T" />.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
#nullable enable
    public override Exception? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => null;

    /// <summary>Writes a specified value as JSON.</summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(nameof(value.Message), value.Message);
        writer.WriteString(nameof(value.Source), value.Source);
        writer.WriteString(nameof(value.StackTrace), value.StackTrace);
        if (value.InnerException != null)
        {
            writer.WriteStartObject(nameof(value.InnerException));
            writer.WriteString(nameof(value.InnerException.Message), value.InnerException.Message);
            writer.WriteString(nameof(value.InnerException.Source), value.InnerException.Source);
            writer.WriteString(nameof(value.InnerException.StackTrace), value.InnerException.StackTrace);
            writer.WriteEndObject();
        }

        writer.WriteEndObject();
    }

    #endregion
}