namespace FullStackHero.DotNext.Core.Extensions;

/// <summary>
///     Methods to remove HTML from strings.
/// </summary>
public static class HtmlRemoval
{
    /// <summary>
    ///     Compiled regular expression for performance.
    /// </summary>
    private static readonly Regex HtmlRegex = new("<.*?>", RegexOptions.Compiled);

    /// <summary>
    ///     Remove HTML from string with Regex.
    /// </summary>
    public static string StripTagsRegex(this string source) => Regex.Replace(source, "<.*?>", string.Empty);

    /// <summary>
    ///     Remove HTML from string with compiled Regex.
    /// </summary>
    public static string StripTagsRegexCompiled(this string source) => HtmlRegex.Replace(source, string.Empty);

    /// <summary>
    ///     Remove HTML tags from string using char array.
    /// </summary>
    public static string StripTagsCharArray(this string source)
    {
        if (string.IsNullOrEmpty(source))
            throw new ArgumentNullException(nameof(source), "Chuỗi nguồn không được null hoặc empty.");

        var array      = new char[source.Length];
        var arrayIndex = 0;
        var inside     = false;

        foreach (var ch in source)
        {
            switch (ch)
            {
                case '<':
                    inside = true;

                    continue;

                case '>':
                    inside = false;

                    continue;
            }

            if (inside) continue;

            array[arrayIndex] = ch;
            arrayIndex++;
        }

        return new string(array, 0, arrayIndex);
    }
}