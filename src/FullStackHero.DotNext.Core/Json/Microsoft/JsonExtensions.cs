using System.Text.Json.Nodes;

namespace FullStackHero.DotNext.Core.Json.Microsoft;

public static class JsonExtensions
{
    #region Private methods

    private static JsonSerializerOptions DefaultSerializerOptions() => new()
    {
        Encoder                     = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // JavaScriptEncoder.Create(UnicodeRanges.All),
        DefaultIgnoreCondition      = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true, // Don't care about the casing
        PropertyNamingPolicy        = JsonNamingPolicy.CamelCase,
        ReferenceHandler            = ReferenceHandler.IgnoreCycles
    };

    #endregion

    #region Public extensions method

    /// <summary>
    ///     Kiểm tra một chuỗi có phải là chuỗi JSON hợp lệ hay không?
    /// </summary>
    /// <param name="json">Chuỗi cần kiểm tra.</param>
    /// <returns></returns>
    public static bool IsWellFormedJson(this string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return false;

        try
        {
            var utf8JsonBytes = Encoding.UTF8.GetBytes(json);
            var reader        = new Utf8JsonReader(new ReadOnlySpan<byte>(utf8JsonBytes), true, new JsonReaderState());

            return JsonDocument.TryParseValue(ref reader, out _);
        }
        catch (JsonException)
        {
            return false;
        }
    }

    public static string? JsonQueryXPath(this string? value, string xpath, JsonSerializerOptions? options = null) =>
        string.IsNullOrWhiteSpace(value) || value == "[]"
            ? null
            : value.Deserialize<JsonElement>(options).GetJsonElement(xpath).GetJsonElementValue();

    /// <summary>
    ///     Access to properties using xpath query. See <see cref="GetJsonElementValue" /> to get value of element.
    /// </summary>
    /// <param name="jsonElement"></param>
    /// <param name="xpath">Ex: data.products.edges.1.node.8.name</param>
    /// <returns></returns>
    public static JsonElement GetJsonElement(this JsonElement jsonElement, string xpath)
    {
        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.Null or JsonValueKind.Undefined:
            case JsonValueKind.Array when jsonElement.GetArrayLength() == 0:
                return default;
        }

        var segments = xpath.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var segment in segments)
        {
            if (int.TryParse(segment, out var index) && jsonElement.ValueKind == JsonValueKind.Array)
            {
                jsonElement = jsonElement.EnumerateArray().ElementAtOrDefault(index);

                if (jsonElement.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
                    return default;

                continue;
            }

            jsonElement = jsonElement.TryGetProperty(segment, out var value) ? value : default;

            if (jsonElement.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
                return default;
        }

        return jsonElement;
    }

    public static string? GetJsonElementValue(this JsonElement jsonElement) => jsonElement.ValueKind != JsonValueKind.Null &&
                                                                               jsonElement.ValueKind != JsonValueKind.Undefined
        ? jsonElement.ToString()
        : default;

    /// <summary>
    ///     Extension methods that access a child <see cref="JsonElement" /> value by property name, returning a nullable value
    ///     if not found.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="name"></param>
    /// <code>
    ///   var doc = JsonSerializer.Deserialize&lt;JsonElement&gt;(raw);  
    ///   var node = doc.Get("data")?.Get("products")?.Get("edges")?.Get(0)?.Get("node");  
    ///   var productIdString = node?.Get("id")?.GetString();
    ///   var originalSrcString = node?.Get("featuredImage")?.Get("originalSrc")?.GetString();
    ///   Int64? someIntegerValue = node?.Get("Size")?.GetInt64();
    ///   </code>
    /// <returns></returns>
    public static JsonElement? Get(this JsonElement element, string name) =>
        element.ValueKind != JsonValueKind.Null && element.ValueKind != JsonValueKind.Undefined && element.TryGetProperty(name, out var value)
            ? value
            : null;

    public static JsonElement? Get(this JsonElement element, int index)
    {
        if (element.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
            return null;

        var value = element.EnumerateArray().ElementAtOrDefault(index);

        return value.ValueKind != JsonValueKind.Undefined ? value : null;
    }

    public static JsonElement? Get(this JsonDocument doc, string name) => doc.RootElement.Get(name);

    public static JsonElement? Get(this JsonDocument doc, int index) => doc.RootElement.Get(index);

    #endregion

    #region Deserialize

    public static async ValueTask<T?> DeserializeAsync<T>(this HttpContent httpContent, JsonSerializerOptions? options = null, CancellationToken ct = default)
    {
        await using var utf8Json = await httpContent.ReadAsStreamAsync(ct).ConfigureAwait(false);

        return await JsonSerializer.DeserializeAsync<T>(utf8Json, options ?? DefaultSerializerOptions(), ct).ConfigureAwait(false);
    }

    public static async ValueTask<object?> DeserializeAsync(this HttpContent httpContent, JsonSerializerOptions? options = null, CancellationToken ct = default)
    {
        await using var utf8Json = await httpContent.ReadAsStreamAsync(ct).ConfigureAwait(false);

        return await JsonSerializer.DeserializeAsync<object>(utf8Json, options ?? DefaultSerializerOptions(), ct).ConfigureAwait(false);
    }

    public static ValueTask<T?> DeserializeAsync<T>(this Stream stream, JsonSerializerOptions? options = null, CancellationToken ct = default) => JsonSerializer.DeserializeAsync<T>(stream, options  ?? DefaultSerializerOptions(), ct);
    public static T?            Deserialize<T>(this string jsonContent, JsonSerializerOptions? options = null)                                 => JsonSerializer.Deserialize<T>(jsonContent, options  ?? DefaultSerializerOptions());
    public static T?            Deserialize<T>(this ReadOnlySpan<byte> utf8Json, JsonSerializerOptions? options = null)                        => JsonSerializer.Deserialize<T>(utf8Json, options     ?? DefaultSerializerOptions());
    public static T?            Deserialize<T>(this ref Utf8JsonReader reader, JsonSerializerOptions? options = null)                          => JsonSerializer.Deserialize<T>(ref reader, options   ?? DefaultSerializerOptions());
    public static T?            Deserialize<T>(this JsonDocument jsonDocument, JsonSerializerOptions? options = null)                          => JsonSerializer.Deserialize<T>(jsonDocument, options ?? DefaultSerializerOptions());
    public static T?            Deserialize<T>(this JsonElement jsonElement, JsonSerializerOptions? options = null)                            => JsonSerializer.Deserialize<T>(jsonElement, options  ?? DefaultSerializerOptions());
    public static T?            Deserialize<T>(this JsonNode jsonNode, JsonSerializerOptions? options = null)                                  => JsonSerializer.Deserialize<T>(jsonNode, options     ?? DefaultSerializerOptions());

    #endregion

    #region Serialize

    public static byte[] SerializeToUtf8Bytes<T>(this T value, JsonSerializerOptions? options = null)                                                            => JsonSerializer.SerializeToUtf8Bytes(value, options   ?? DefaultSerializerOptions());
    public static byte[] SerializeToUtf8Bytes(this object value, JsonSerializerOptions? options = null)                                                          => JsonSerializer.SerializeToUtf8Bytes(value, options   ?? DefaultSerializerOptions());
    public static string SerializeToString<T>(this T value, JsonSerializerOptions? options = null)                                                               => JsonSerializer.Serialize(value, options              ?? DefaultSerializerOptions());
    public static string SerializeToString(this object value, JsonSerializerOptions? options = null)                                                             => JsonSerializer.Serialize(value, options              ?? DefaultSerializerOptions());
    public static Task   SerializeToStreamAsync<TValue>(this TValue value, Stream stream, JsonSerializerOptions? options = null, CancellationToken ct = default) => JsonSerializer.SerializeAsync(stream, value, options ?? DefaultSerializerOptions(), ct);
    public static Task   SerializeToStreamAsync(this object value, Stream stream, JsonSerializerOptions? options = null, CancellationToken ct = default)         => JsonSerializer.SerializeAsync(stream, value, options ?? DefaultSerializerOptions(), ct);

    public static Task SerializeToFileAsync<T>(this T value, string path, JsonSerializerOptions? options = null, CancellationToken ct = default)
    {
        using var fs = File.Create(path, 4096, FileOptions.Asynchronous);

        return JsonSerializer.SerializeAsync(fs, value, options ?? DefaultSerializerOptions(), ct);
    }

    public static Task SerializeToFileAsync(this object value, string path, JsonSerializerOptions? options = null, CancellationToken ct = default)
    {
        using var fs = File.Create(path, 4096, FileOptions.Asynchronous);

        return JsonSerializer.SerializeAsync(fs, value, options ?? DefaultSerializerOptions(), ct);
    }

    public static Task SerializeToFileAppendAsync<TValue>(this TValue value, string path, JsonSerializerOptions? options, CancellationToken ct = default)
    {
        var json = value.SerializeToString(options);

        return File.AppendAllTextAsync(path, $"{json}{Environment.NewLine}", ct);
    }

    public static Task SerializeToFileAppendAsync(this object value, string path, JsonSerializerOptions? options, CancellationToken ct = default)
    {
        var json = value.SerializeToString(options);

        return File.AppendAllTextAsync(path, $"{json}{Environment.NewLine}", ct);
    }

    #endregion
}