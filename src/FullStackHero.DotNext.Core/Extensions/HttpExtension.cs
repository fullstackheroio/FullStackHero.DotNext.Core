namespace FullStackHero.DotNext.Core.Extensions;

public static class HttpExtension
{
    #region Enum

    public enum Destination : byte
    {
        Decimal = 1,
        Hex     = 2
    }

    #endregion

    #region Private methods

    /// <summary>
    ///     Convert a UTF-16 encoded .Net string into an array of UTF-32 encoding Unicode chars
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private static IEnumerable<uint> StringToArrayOfUtf32Chars(string source)
    {
        var bytes                                                           = Encoding.UTF32.GetBytes(source);
        var utf32Chars                                                      = new uint[bytes.Length / sizeof(uint)];
        for (int i = 0, j = 0; i < bytes.Length; i += 4, ++j) utf32Chars[j] = BitConverter.ToUInt32(bytes, i);

        return utf32Chars;
    }

    #endregion

    #region Public extension methods

    /// <summary>
    ///     Chuyển đổi một cuỗi unicode sang HTML encode.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <remarks>
    ///     Reference:
    ///     https://stackoverflow.com/questions/4663538/how-to-convert-unicode-character-to-its-escaped-ascii-equivalent-in-c-sharp
    /// </remarks>
    /// <example>
    ///     Nguyễn Xuân Huyên >> Nguy&amp;#7877;n Xu&amp;#226;n Huy&amp;#234;n
    /// </example>
    /// <returns></returns>
    public static string HtmlEncodeEx(this string source, Destination destination = Destination.Decimal)
    {
        if (string.IsNullOrWhiteSpace(source))
            return source;

        /* In the .Net world, strings are UTF-16 encoded.That means that Unicode codepoints greater than 0x007F
         * are encoded in the string as 2 - character digraphs.So to properly turn them into HTML numeric
         * character references(decimal or hex), we first need to get the UTF - 32 encoding.
         * */
        var utf32Chars = StringToArrayOfUtf32Chars(source);
        var sb         = new StringBuilder(2000); // set a reasonable initial size for the buffer

        // iterate over the utf-32 encoded characters
        foreach (var codePoint in utf32Chars)
        {
            // if the code point is greater than 0x7F, it gets turned into an HTML numerica character reference
            if (codePoint > 0x0000007F)
            {
                switch (destination)
                {
                    // decimal escape sequence
                    case Destination.Decimal:
                        sb.AppendFormat("&#{0};", codePoint);

                        break;

                    // hex escape sequence
                    case Destination.Hex:
                        sb.AppendFormat("&#x{0:X};", codePoint);

                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(destination), destination, null);
                }

                continue;
            }
            // if less than or equal to 0x7F, it goes into the string as-is,
            // except for the 5 SGML/XML/HTML reserved characters. You might
            // want to also escape all the ASCII control characters (those chars
            // in the range 0x00 - 0x1F).

            // convert the unit to an UTF-16 character
            var ch = Convert.ToChar(codePoint);

            // do the needful.
            switch (ch)
            {
                case '"':
                    sb.Append("&quot;");

                    break;

                case '\'':
                    sb.Append("&apos;");

                    break;

                case '&':
                    sb.Append("&amp;");

                    break;

                case '<':
                    sb.Append("&lt;");

                    break;

                case '>':
                    sb.Append("&gt;");

                    break;

                default:
                    sb.Append(ch.ToString());

                    break;
            }
        }

        // return the escaped, utf-16 string back to the caller.
        return sb.ToString();
    }

    public static string HtmlEncode(this string source) => WebUtility.HtmlEncode(source);
    public static string HtmlDecode(this string source) => WebUtility.HtmlDecode(source);
    public static string UrlEncode(this string source)  => WebUtility.UrlEncode(source);
    public static string UrlDecode(this string source)  => WebUtility.UrlDecode(source);

    #endregion
}