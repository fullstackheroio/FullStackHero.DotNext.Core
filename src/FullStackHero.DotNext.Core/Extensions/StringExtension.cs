using FullStackHero.DotNext.Core.Exceptions;

namespace FullStackHero.DotNext.Core.Extensions;

/// <summary>
///     This class is an extension for strings. No need to call it directly.
/// </summary>
public static class StringExtension
{
    #region Private fields

    private static readonly Random Rnd     = new();
    private const           string Chars   = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const           string Numbers = "0123456789";

    private static readonly Dictionary<string, string> HtmlMnemonics = new()
    {
        { "apos", "'" },
        { "quot", "\"" },
        { "amp", "&" },
        { "lt", "<" },
        { "gt", ">" }
    };

    private static readonly string[] VietnameseSigns =
    {
        "aAeEoOuUiIdDyY",
        "áàạảãâấầậẩẫăắằặẳẵ",
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
        "éèẹẻẽêếềệểễ",
        "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ",
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
        "úùụủũưứừựửữ",
        "ÚÙỤỦŨƯỨỪỰỬỮ",
        "íìịỉĩ",
        "ÍÌỊỈĨ",
        "đ",
        "Đ",
        "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
    };

    #endregion

    #region Validation

    public static bool IsNotNullOrWhiteSpace(this string source) => !string.IsNullOrWhiteSpace(source);
    public static bool IsNotNullOrEmpty(this string? source)     => !string.IsNullOrEmpty(source);
    public static bool IsNullOrWhiteSpace(this string? source)   => string.IsNullOrWhiteSpace(source);
    public static bool IsNullOrEmpty(this string source)         => string.IsNullOrEmpty(source);

    /// <summary>
    ///     Kiểm tra một chuỗi có chứa trong tất cả các phần tử của một tập hợp hay không?
    /// </summary>
    /// <param name="source">Chuỗi cần kiểm tra.</param>
    /// <param name="container"></param>
    /// <returns></returns>
    public static bool ContainsAll(this string source, IEnumerable<string> container) => container.All(source.Contains);

    public static bool ContainsAll(this string source, IEnumerable<string> container, StringComparison comparison) => container.All(str => str.Contains(source, comparison));

    /// <summary>
    ///     Kiểm tra một chuỗi có nằm trong ít nhất một phần tử của tập hợp hay không?
    /// </summary>
    /// <param name="source">Chuỗi cần kiểm tra.</param>
    /// <param name="container"></param>
    /// <returns></returns>
    public static bool ContainsOne(this string source, IEnumerable<string> container) => container.Any(source.Contains);

    public static bool ContainsOne(this string source, IEnumerable<string> container, StringComparison comparison) => container.Any(str => str.Contains(source, comparison));

    public static float   IsFloat(this string source)                  => IsFloat(source, out var f) ? f : default;
    public static bool    IsFloat(this string source, out float f)     => float.TryParse(source, out f);
    public static double  IsDouble(this string source)                 => IsDouble(source, out var d) ? d : default;
    public static bool    IsDouble(this string source, out double d)   => double.TryParse(source, out d);
    public static decimal IsDecimal(this string source)                => IsDecimal(source, out var m) ? m : default;
    public static bool    IsDecimal(this string source, out decimal m) => decimal.TryParse(source, out m);
    public static int     IsInt32(this string source)                  => IsInt32(source, out var n) ? n : default;
    public static bool    IsInt32(this string source, out int n)       => int.TryParse(source, out n);
    public static long    IsInt64(this string source)                  => IsInt64(source, out var l) ? l : default;
    public static bool    IsInt64(this string source, out long l)      => long.TryParse(source, out l);
    public static bool    IsNumeric(this string source)                => source.All(c => char.IsDigit(c) || c == 46);

    public static bool IsNumeric(this string source, out int number)
    {
        number = 0;

        return !string.IsNullOrWhiteSpace(source) && int.TryParse(source.Trim(), out number);
    }

    public static bool IsDateTime(this string source, string format = "dd/MM/yyyy") => source.IsDateTime(out _, format);

    public static bool IsDateTime(this string source, out DateTime dateTime, string format = "dd/MM/yyyy") =>
        DateTime.TryParseExact(source, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);

    /// <summary>
    ///     Phân tích 1 string là số >=0 thành kiểu số Int > 0.
    ///     Trả về -1 nếu string không phân tích thành số được.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static int ToUInt(this string source) => int.TryParse(source, out var result) ? result : -1;

    public static int ToUInt(this string source, out int result) => int.TryParse(source, out result) ? result : -1;

    #endregion

    #region Additional functions

    public static string ToSpacing(this string source, int count = 1, char delimiter = ' ')
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source), "Chuỗi nguồn không được null.");

        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Số ký tự muốn chèn vào chuỗi phải lớn hơn hoặc bằng 0.");

        var delimiterStr = new string(delimiter, count);
        var sb           = new StringBuilder();
        foreach (var ch in source) sb.Append($"{ch}{delimiterStr}");

        return sb.Remove(sb.Length - count, count).ToString();
    }

    public static string? Quote(this string source)
    {
        if (source.IsNullOrWhiteSpace()) return null;

        return Constant.Q + source.Replace(Constant.Q, Constant.Eq) + Constant.Q;
    }

    public static string QuoteSingle(this string source) => Constant.QuoteC + source + Constant.QuoteC;

    public static string Last(this string source, int length)
    {
        if (string.IsNullOrWhiteSpace(source)) return string.Empty;

        return source.Length <= length
            ? source
            : source[^length..];
    }

    public static int CountStringOccurrences(this string destination, string search)
    {
        var counter    = 0;
        var startIndex = 0;
        int i;

        while ((i = destination.IndexOf(search, startIndex, StringComparison.Ordinal)) != -1)
        {
            startIndex = i + search.Length;
            ++counter;
        }

        return counter;
    }

    public static bool IsCharIs(this char chr, bool isLetter, bool isDigit, params char[] accepts)
    {
        if (isLetter && char.IsLetter(chr)) return true;
        if (isDigit  && char.IsDigit(chr)) return true;

        return accepts.Length > 0 && accepts.Contains(chr);
    }

    public static string? WordFromIndex(this string source, int index, params char[] accepts) =>
        WordFromIndex(source, index, true, accepts);

    public static string? WordFromIndex(this string source, int index, bool acceptDigit, params char[] accepts)
    {
        if (string.IsNullOrWhiteSpace(source) || index == 0 || !IsCharIs(source[index], true, acceptDigit, accepts))
            return null;

        var startIndex = index;
        while (index < source.Length && IsCharIs(source[index], true, acceptDigit, accepts)) ++index;
        if (index >= source.Length) index = source.Length - 1;
        if (!IsCharIs(source[index], true, acceptDigit, accepts)) --index;
        while (startIndex > 0 && IsCharIs(source[startIndex], true, acceptDigit, accepts)) --startIndex;
        if (startIndex <= 0) index = 0;
        if (!IsCharIs(source[startIndex], true, acceptDigit, accepts)) ++startIndex;

        return source.Substring(startIndex, index - startIndex + 1);
    }

    public static string FlatLine(this string original)
    {
        if (original.IsNullOrWhiteSpace()) return string.Empty;

        return original.Replace(Constant.R, Constant.N)
                       .Replace(Constant.Nn, Constant.N)
                       .Replace(Constant.N, Constant.Space);
    }

    public static string MaxLength(this string source, int length) => source.Length <= length - 3 ? source : source.Substring(0, length - 3) + Constant.Etc;

    public static string PadCenter(this string source, int maxLength)
    {
        try
        {
            var num = maxLength - source.Length;

            return new string(Constant.SpaceC, num / 2) + source +
                   new string(Constant.SpaceC, (int)(num / 2.0 + 0.5));
        }
        catch
        {
            return string.Empty;
        }
    }

    public static string SplitCamelCase(this string source)
    {
        const string pattern1 = "(\\P{Ll})(\\P{Ll}\\p{Ll})";
        const string pattern2 = "(\\p{Ll})(\\P{Ll})";
        const string replace  = "$1 $2";
        var          result   = Regex.Replace(source, pattern1, replace);

        return Regex.Replace(result, pattern2, replace);
    }

    public static string ToTitleCase(this string souce)
    {
        if (string.IsNullOrWhiteSpace(souce))
            throw new ArgumentNullException(nameof(souce), "Chuỗi nguồn null hoặc empty.");

        return CultureInfo.InstalledUICulture.TextInfo.ToTitleCase(souce.ToLowerInvariant());
    }

    public static string ToTitleCase2(this string souce)
    {
        if (string.IsNullOrWhiteSpace(souce))
            throw new ArgumentNullException(nameof(souce), "Chuỗi nguồn null hoặc empty.");

        static IEnumerable<char> CharsToTitleCase(string s)
        {
            var newWord = true;

            foreach (var c in s)
            {
                if (newWord)
                {
                    yield return char.ToUpper(c);

                    newWord = false;

                    continue;
                }

                yield return char.ToLower(c);

                if (c == ' ') newWord = true;
            }
        }

        return new string(CharsToTitleCase(souce).ToArray());
    }

    /// <summary>
    ///     Lấy một chuỗi đứng trước chuỗi
    ///     <param name="str"></param>
    ///     cho trước.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="str"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static string Before(this string source, string str, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        var posA = source.IndexOf(str, comparison);

        return posA == -1 ? string.Empty : source[..posA];
    }

    /// <summary>
    ///     Lấy một chuỗi đứng sau chuỗi
    ///     <param name="str"></param>
    ///     cho trước.
    /// </summary>
    public static string After(this string source, string str, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        var posA = source.LastIndexOf(str, comparison);

        if (posA == -1) return string.Empty;

        var adjustedPosA = posA + str.Length;

        return adjustedPosA >= source.Length ? string.Empty : source.Substring(adjustedPosA);
    }

    public static string UnEscape(this string source)
    {
        const string pattern = "\\[rnt\"]";

        return Regex.Replace(source, pattern, m => m.Value switch
        {
            Constant.Er   => Constant.R,
            Constant.En   => Constant.N,
            Constant.Etab => Constant.Tab,
            Constant.Eq   => Constant.Q,
            _             => m.Value
        });
    }

    /// <summary>
    ///     Checks for the presence of a substring in a string, excluding the registry, through a comparison:
    ///     <see cref="StringComparison.OrdinalIgnoreCase" />.
    /// </summary>
    /// <param name="source">Line</param>
    /// <param name="value">The substring to look for in the source string</param>
    /// <returns>Вернет <langword>true</langword> </returns>
    public static bool ContainsInsensitive(this string source, string value) => source.IndexOf(value, StringComparison.OrdinalIgnoreCase) != -1;

    public static string ToAscii(this string source) =>
        string.Join(string.Empty, source.Normalize(NormalizationForm.FormD)
                                        .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));

    /// <summary>
    ///     Replace in Html-Entites on symbols
    /// </summary>
    /// <param name="str">String in which replacement will be made.</param>
    /// <returns>A string replaced with HTML-entities.</returns>
    /// <remarks>Replace only with the following mnemonics: apos, quot, amp, lt и gt. And all types codes.</remarks>
    public static string ReplaceEntities(this string str)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;

        var regex = new Regex(@"(\&(?<text>\w{1,4})\;)|(\&#(?<code>\w{1,4})\;)", RegexOptions.Compiled);

        var result = regex.Replace(str, match =>
        {
            if (match.Groups["text"].Success)
            {
                if (HtmlMnemonics.TryGetValue(match.Groups["text"].Value, out var value)) return value;
            }
            else if (match.Groups["code"].Success)
            {
                var code = int.Parse(match.Groups["code"].Value);

                return ((char)code).ToString();
            }

            return match.Value;
        });

        return result;
    }

    /// <summary>
    ///     Replace in string Unicode-entities on symbols.
    /// </summary>
    /// <param name="source">String in which replacement will be made.</param>
    /// <returns>>A string replaced with Unicode-entites.</returns>
    /// <remarks>Unicode-enities: \u2320 or \U044F</remarks>
    public static string ReplaceUnicode(this string source)
    {
        if (string.IsNullOrEmpty(source)) return string.Empty;

        var regex = new Regex(@"\\u(?<code>[0-9a-f]{4})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        var result = regex.Replace(source, match =>
        {
            var code = int.Parse(match.Groups["code"].Value, NumberStyles.HexNumber);

            return ((char)code).ToString();
        });

        return result;
    }

    /// <summary>
    ///     Generate a random n character alphanumeric string.
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string RandomString(int length) => new(Enumerable.Repeat(Chars, length).Select(s => s[Rnd.Next(s.Length)]).ToArray());

    public static string RandomString2(int length)
    {
        var chars                                       = new char[length];
        for (var i = 0; i < chars.Length; i++) chars[i] = Chars[Rnd.Next(Chars.Length)];

        return new string(chars);
    }

    public static string GetUniqueString(int length)
    {
        var chars = Chars.ToCharArray();
        var data  = new byte[length];

        using (var crypto = RandomNumberGenerator.Create())
        {
            crypto.GetBytes(data);
        }

        var result = new StringBuilder(length);
        foreach (var b in data) result.Append(chars[b % chars.Length]);

        return result.ToString();
    }

    public static string RandomNumber(int length) => new(Enumerable.Repeat(Numbers, length).Select(s => s[Rnd.Next(s.Length)]).ToArray());

    #endregion

    #region Keep

    public static string? Keep(this string? source, bool letterOrDigit, params char[] chars)
    {
        if (source == null) return null;

        return letterOrDigit
            ? new string(source.Where(c => char.IsLetterOrDigit(c) || chars.Contains(c)).ToArray())
            : new string(source.Where(chars.Contains).ToArray());
    }

    public static string? KeepLetterOrDigit(this string? source) => source != null ? new string(source.Where(char.IsLetterOrDigit).ToArray()) : null;
    public static string? KeepDigit(this string? source)         => source != null ? new string(source.Where(char.IsDigit).ToArray()) : null;
    public static string? KeepLetter(this string? source)        => source != null ? new string(source.Where(char.IsLetter).ToArray()) : null;

    #endregion

    #region Remove & Replace

    public static string Replace(this string source, NameValueCollection nvc) =>
        nvc.Cast<string>().Aggregate(source, (current, key) => current.Replace(key, nvc[key]));

    public static string Replace(this string source, Dictionary<string, string> nvc) =>
        nvc.Aggregate(source, (current, pair) => current.Replace(pair.Key, pair.Value));

    public static string Replace(this string source, string ch, params string[] str) =>
        str.Aggregate(source, (current, s) => current.Replace(s, ch));

    public static bool RemovePrefix(this string source, string prefix, ref string destination)
    {
        if (string.IsNullOrWhiteSpace(source)) return false;
        if (string.IsNullOrWhiteSpace(prefix)) return false;
        if (!source.StartsWith(prefix)) return false;

        destination = source[prefix.Length..];

        return true;
    }

    public static string? RemovePrefix(this string? source, string prefix)
    {
        if (string.IsNullOrWhiteSpace(source)) return null;
        if (string.IsNullOrWhiteSpace(prefix)) return source;

        return source.StartsWith(prefix) ? source[prefix.Length..] : source;
    }

    public static bool RemoveSuffix(this string source, string suffix, ref string destination)
    {
        if (string.IsNullOrWhiteSpace(source)) return false;
        if (string.IsNullOrWhiteSpace(suffix)) return false;
        if (!source.EndsWith(suffix)) return false;

        destination = source[..^suffix.Length];

        return true;
    }

    public static string? RemoveSuffix(this string? source, string suffix)
    {
        if (string.IsNullOrWhiteSpace(source)) return null;
        if (string.IsNullOrWhiteSpace(suffix)) return source;

        return source.EndsWith(suffix) ? source[..^suffix.Length] : source;
    }

    /// <summary>
    ///     Hàm hỗ trợ chuyển đổi chuỗi Tiếng Việt có dấu sang không dấu
    /// </summary>
    /// <param name="source">Chuỗi cần chuyển</param>
    /// <returns></returns>
    public static string ToUnsign(this string source)
    {
        if (string.IsNullOrWhiteSpace(source)) return string.Empty;

        var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
        var temp  = source.Normalize(NormalizationForm.FormD);

        return regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
    }

    public static string ToVietnameseUnsign(this string source)
    {
        if (string.IsNullOrWhiteSpace(source)) return string.Empty;

        for (var i = 1; i < VietnameseSigns.Length; i++)
        for (var j = 0; j < VietnameseSigns[i].Length; j++)
            source = source.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);

        return source;
    }

    #endregion

    #region Split

    public static string[]? Split(this string source, string splitter, int count) => source.Split(splitter, StringSplitOptions.None, count);

    public static string[]? Split(this string source, string splitter, StringSplitOptions options = StringSplitOptions.None, int count = 0)
    {
        if (string.IsNullOrEmpty(source)) return null;

        return count > 0
            ? source.Split(new[] { splitter }, count, options)
            : source.Split(new[] { splitter }, options);
    }

    public static string[]? Split(this string source, char splitter, int count) => source.Split(splitter, StringSplitOptions.None, count);

    public static string[]? Split(this string source, char splitter, StringSplitOptions options = StringSplitOptions.None, int count = 0)
    {
        if (string.IsNullOrEmpty(source)) return null;

        return count > 0
            ? source.Split(new[] { splitter }, count, options)
            : source.Split(new[] { splitter }, options);
    }

    public static string[] Split(this string source, params char[] splitter)                               => source.Split(splitter);
    public static string[] Split(this string source, int count, params char[] splitter)                    => source.Split(splitter, count);
    public static string[] Split(this string source, int count, params string[] splitter)                  => source.Split(splitter, count, StringSplitOptions.None);
    public static string[] Split(this string source, StringSplitOptions options, params char[] splitter)   => source.Split(splitter, options);
    public static string[] Split(this string source, bool removeEmpty, params string[] splitter)           => source.Split(splitter, removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
    public static string[] Split(this string source, StringSplitOptions options, params string[] splitter) => source.Split(splitter, options);
    public static string[] Split(this string source, params string[] splitter)                             => source.Split(splitter, StringSplitOptions.None);

    #endregion

    #region Substrings: A few lines

    /// <summary>
    ///     Cuts multiple lines between two substrings. If there are no matches, it will return an empty array.
    /// </summary>
    /// <param name="source">String where to look for substrings</param>
    /// <param name="left">Start substring</param>
    /// <param name="right">End substring</param>
    /// <param name="startIndex">Search starting from index</param>
    /// <param name="comparison">String Comparison Method</param>
    /// <param name="limit">Maximum number of substrings to search</param>
    /// <exception cref="ArgumentNullException">Occurs if one of the parameters is an empty string or <keyword>null</keyword>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Occurs if the start index exceeds the row length.</exception>
    /// <returns>Returns an array of substrings that fall under the pattern or an empty array if there are no matches.</returns>
    public static string[] SubstringsOrEmpty(this string source,
                                             string left,
                                             string right,
                                             int startIndex = 0,
                                             StringComparison comparison = StringComparison.OrdinalIgnoreCase,
                                             int limit = 0)
    {
        #region Verification of parameters

        if (string.IsNullOrEmpty(source))
            return Array.Empty<string>();

        if (string.IsNullOrEmpty(left))
            throw new ArgumentNullException(nameof(left), "Chuỗi kiểm tra bên trái không được null hoặc empty.");

        if (string.IsNullOrEmpty(right))
            throw new ArgumentNullException(nameof(right), "Chuỗi kiểm tra bên phải không được null hoặc empty.");

        if (startIndex < 0 || startIndex >= source.Length)
            throw new ArgumentOutOfRangeException(nameof(startIndex), "Chỉ số bắt đầu lớn hơn chiều dài của chuỗi.");

        #endregion

        var currentStartIndex = startIndex;
        var current           = limit;
        var strings           = new List<string>();

        while (true)
        {
            if (limit > 0)
            {
                --current;

                if (current < 0)
                    break;
            }

            // We are looking for the beginning of the position of the left substring.
            var leftPosBegin = source.IndexOf(left, currentStartIndex, comparison);

            if (leftPosBegin == -1)
                break;

            // We calculate the end of the position of the left substring.
            var leftPosEnd = leftPosBegin + left.Length;
            // We are looking for the beginning of the position of the right line.
            var rightPos = source.IndexOf(right, leftPosEnd, comparison);

            if (rightPos == -1)
                break;

            // We calculate the length of the found substring.
            var length = rightPos - leftPosEnd;
            strings.Add(source.Substring(leftPosEnd, length));
            // We calculate the end of the position of the right substring.
            currentStartIndex = rightPos + right.Length;
        }

        return strings.ToArray();
    }

    /// <inheritdoc cref="SubstringsOrEmpty" />
    /// <summary>
    ///     Cuts multiple lines between two substrings. If there are no matches, will return <keyword>null</keyword>.
    ///     <remarks>
    ///         Created for convenience, for writing exceptions through ?? ternary operator.
    ///     </remarks>
    ///     <example>
    ///         str.Substrings("<tag>","</tag>") ?? throw new Exception("Not found row");
    ///     </example>
    ///     <remarks>
    ///         Do not forget about the function <see cref="SubstringsEx" /> - which already throws an exception
    ///         <see cref="SubstringException" /> in case there will be no match.
    ///     </remarks>
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="fallback">Value if substrings are not found</param>
    /// <param name="source"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="startIndex"></param>
    /// <param name="comparison"></param>
    /// <returns>Returns an array of substrings that fall under the pattern or <keyword>null</keyword>.</returns>
    public static string[] Substrings(this string source,
                                      string left,
                                      string right,
                                      int startIndex = 0,
                                      StringComparison comparison = StringComparison.OrdinalIgnoreCase,
                                      int limit = 0,
                                      string[] fallback = null!)
    {
        var result = SubstringsOrEmpty(source, left, right, startIndex, comparison, limit);

        return result.Length > 0 ? result : fallback;
    }

    /// <inheritdoc cref="SubstringsOrEmpty" />
    /// <summary>
    ///     Cuts multiple lines between two substrings. If there are no matches, an exception will be thrown.
    ///     <see cref="SubstringException" />.
    /// </summary>
    /// <exception cref="SubstringException">Will be thrown if no matches were found</exception>
    /// <returns>
    ///     Returns an array of substrings that fall under the pattern or throws an exception
    ///     <see cref="SubstringException" /> if no matches were found.
    /// </returns>
    public static string[] SubstringsEx(this string source,
                                        string left,
                                        string right,
                                        int startIndex = 0,
                                        StringComparison comparison = StringComparison.OrdinalIgnoreCase,
                                        int limit = 0)
    {
        var result = SubstringsOrEmpty(source, left, right, startIndex, comparison, limit);

        return result.Length != 0 ? result : throw new SubstringException($"Substrings not found. Left: \"{left}\". Right: \"{right}\".");
    }

    #endregion

    #region Substring: One substring. Direct order (left to right)

    /// <summary>
    ///     Cuts a single line between two substrings. If there are no matches, will return <paramref name="fallback" /> or by
    ///     default <keyword>null</keyword>.
    ///     <remarks>
    ///         Created for convenience, for writing exceptions through ?? ternary operator.
    ///     </remarks>
    ///     <example>
    ///         str.Between("<tag>","</tag>") ?? throw new Exception("Не найдена строка");
    ///     </example>
    ///     <remarks>
    ///         Do not forget about the function <see cref="SubstringEx" /> - which already throws an exception
    ///         <see cref="SubstringException" /> in case there will be no match.
    ///     </remarks>
    /// </summary>
    /// <param name="source">String where to look for substrings</param>
    /// <param name="left">Start substring</param>
    /// <param name="right">End substring</param>
    /// <param name="startIndex">Search starting from index</param>
    /// <param name="comparison">String Comparison Method</param>
    /// <param name="fallback">Value if substring not found</param>
    /// <returns>
    ///     Returns a string between two substrings or <paramref name="fallback" /> (default <keyword>null</keyword>).
    /// </returns>
    public static string Substring(this string source,
                                   string left,
                                   string right,
                                   int startIndex = 0,
                                   StringComparison comparison = StringComparison.OrdinalIgnoreCase,
                                   string fallback = null!)
    {
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right) || startIndex < 0 || startIndex >= source.Length)
            return fallback;

        // We are looking for the beginning of the position of the left substring.
        var leftPosBegin = source.IndexOf(left, startIndex, comparison);

        if (leftPosBegin == -1)
            return fallback;

        // We calculate the end of the position of the left substring.
        var leftPosEnd = leftPosBegin + left.Length;
        // We are looking for the beginning of the position of the right line.
        var rightPos = source.IndexOf(right, leftPosEnd, comparison);

        return rightPos != -1 ? source[leftPosEnd..rightPos] : fallback;
    }

    /// <inheritdoc cref="Substring" />
    /// <summary>
    ///     Cuts a single line between two substrings. If there are no matches, it will return an empty string.
    /// </summary>
    /// <returns>Returns a string between two substrings. If there are no matches, will return an empty string.</returns>
    public static string SubstringOrEmpty(this string source,
                                          string left,
                                          string right,
                                          int startIndex = 0,
                                          StringComparison comparison = StringComparison.OrdinalIgnoreCase) =>
        Substring(source, left, right, startIndex, comparison, string.Empty);

    /// <inheritdoc cref="Substring" />
    /// <summary>
    ///     Cuts a single line between two substrings. If there are no matches, an exception will be thrown.
    ///     <see cref="SubstringException" />.
    /// </summary>
    /// <exception cref="SubstringException">Will be thrown if no matches were found</exception>
    /// <returns>
    ///     Returns a string between two substrings or throws an exception <see cref="SubstringException" /> if no matches were
    ///     found.
    /// </returns>
    public static string SubstringEx(this string source,
                                     string left,
                                     string right,
                                     int startIndex = 0,
                                     StringComparison comparison = StringComparison.OrdinalIgnoreCase) =>
        Substring(source, left, right, startIndex, comparison)
        ?? throw new SubstringException($"Substring not found. Left: \"{left}\". Right: \"{right}\".");

    #endregion

    #region Cutting one substring. Reverse order (right to left)

    /// <inheritdoc cref="Substring" />
    /// <summary>
    ///     Cut one line between two substrings, just starting the search from the end. If there are no matches, will return
    ///     <paramref name="notFoundValue" /> или по-умолчанию <keyword>null</keyword>.
    ///     <remarks>
    ///         Created for convenience, for writing exceptions through ?? ternary operator.
    ///     </remarks>
    ///     <example>
    ///         str.BetweenLast("<tag>","</tag>") ?? throw new Exception("Not found row");
    ///     </example>
    ///     <remarks>
    ///         Do not forget about the function <see cref="SubstringLastEx" /> - which already throws an exception
    ///         <see cref="SubstringException" /> in case there will be no match.
    ///     </remarks>
    /// </summary>
    public static string SubstringLast(this string source,
                                       string right,
                                       string left,
                                       int startIndex = -1,
                                       StringComparison comparison = StringComparison.OrdinalIgnoreCase,
                                       string notFoundValue = null!)
    {
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(right) || string.IsNullOrEmpty(left) ||
            startIndex < -1              || startIndex >= source.Length)
            return notFoundValue;

        if (startIndex == -1)
            startIndex = source.Length - 1;

        // We are looking for the beginning of the position of the right substring from the end of the line
        var rightPosBegin = source.LastIndexOf(right, startIndex, comparison);

        if (rightPosBegin == -1 || rightPosBegin == 0) // в обратном поиске имеет смысл проверять на 0
            return notFoundValue;

        // We calculate the beginning of the position of the left substring
        var leftPosBegin = source.LastIndexOf(left, rightPosBegin - 1, comparison);

        // If the left end is not found or the right and left substring are glued together - return an empty string
        if (leftPosBegin == -1 || rightPosBegin - leftPosBegin == 1)
            return notFoundValue;

        var leftPosEnd = leftPosBegin + left.Length;

        return source[leftPosEnd..rightPosBegin];
    }

    /// <inheritdoc cref="SubstringOrEmpty" />
    /// <summary>
    ///     Cut one line between two substrings, just starting the search from the end. If there are no matches, it will return
    ///     an empty string.
    /// </summary>
    public static string SubstringLastOrEmpty(this string source,
                                              string right,
                                              string left,
                                              int startIndex = -1,
                                              StringComparison comparison = StringComparison.OrdinalIgnoreCase) =>
        SubstringLast(source, right, left, startIndex, comparison, string.Empty);

    /// <inheritdoc cref="SubstringEx" />
    /// <summary>
    ///     Cut one line between two substrings, just starting the search from the end. If there are no matches, an exception
    ///     will be thrown. <see cref="SubstringException" />.
    /// </summary>
    public static string SubstringLastEx(this string source,
                                         string right,
                                         string left,
                                         int startIndex = -1,
                                         StringComparison comparison = StringComparison.OrdinalIgnoreCase) =>
        SubstringLast(source, right, left, startIndex, comparison)
        ?? throw new SubstringException($"StringBetween not found. Right: \"{right}\". Left: \"{left}\".");

    #endregion

    #region TTT's between string

    public static IEnumerable<string> BetweenAll(this string source, string begin, string end, bool includeSearcher = false)
    {
        var currentStartPos = 0;

        for (var result = Between(source, begin, end, ref currentStartPos, includeSearcher);
             result != null;
             result = Between(source, begin, end, ref currentStartPos, includeSearcher))
            yield return result;
    }

    public static string? BetweenNested(this string source, string begin, string end, ref int startPos)
    {
        if (string.IsNullOrEmpty(source))
            throw new ArgumentNullException(nameof(source), "Chuỗi nguồn null hoặc empty.");

        if (string.IsNullOrEmpty(begin))
            throw new ArgumentNullException(nameof(begin), "Chuỗi kiểm tra bên trái không được null hoặc empty.");

        if (string.IsNullOrEmpty(end))
            throw new ArgumentNullException(nameof(end), "Chuỗi kiểm tra bên phải không được null hoặc empty.");

        if (startPos < 0 || startPos >= source.Length)
            throw new ArgumentOutOfRangeException(nameof(startPos), "Chỉ số bắt đầu không được nhỏ hơn < 0, hoặc lớn hơn chiều dài của chuỗi.");

        var pos1 = source.IndexOf(begin, startPos, StringComparison.Ordinal);

        if (pos1 < 0) return null;

        var pos2 = source.IndexOf(end, pos1 + begin.Length, StringComparison.Ordinal);

        if (pos2 < 0 || pos2 <= pos1) return null;

        var pos3 = source.IndexOf(begin, pos1 + begin.Length, pos2 - pos1 - begin.Length, StringComparison.Ordinal);

        while (pos3 >= 0)
        {
            pos2 = source.IndexOf(end, pos2   + end.Length, StringComparison.Ordinal);
            pos3 = source.IndexOf(begin, pos3 + begin.Length, pos2 - pos3 - begin.Length, StringComparison.Ordinal);
        }

        var startIndex = pos1 + begin.Length;

        if (startIndex > pos2) return null;

        startPos = pos2 + end.Length;

        return startIndex == pos2 ? string.Empty : source[startIndex..pos2];
    }

    public static string? Between(this string source, string begin, string end, bool includeSearcher = false, bool ignoreCase = true, char? escapeChar = null)
    {
        var nextPos = 0;

        return Between(source, begin, end, ref nextPos, includeSearcher, ignoreCase, escapeChar);
    }

    public static string? Between(this string source, string begin, string end, ref int nextPos, bool includeSearcher = false, bool ignoreCase = true, char? escapeChar = null)
    {
        if (string.IsNullOrEmpty(source))
            throw new ArgumentNullException(nameof(source), "Chuỗi nguồn null hoặc empty.");

        if (string.IsNullOrEmpty(begin))
            throw new ArgumentNullException(nameof(begin), "Chuỗi kiểm tra bên trái không được null hoặc empty.");

        if (string.IsNullOrEmpty(end))
            throw new ArgumentNullException(nameof(end), "Chuỗi kiểm tra bên phải không được null hoặc empty.");

        if (nextPos < 0 || nextPos >= source.Length)
            throw new ArgumentOutOfRangeException(nameof(nextPos), "Chỉ số bắt đầu không được nhỏ hơn < 0, hoặc lớn hơn chiều dài của chuỗi.");

        var cpr      = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        var beginPos = source.IndexOf(begin, nextPos, cpr);
        int y;

        if (escapeChar.HasValue)
        {
            y = beginPos + begin.Length;

            while (true)
            {
                y = source.IndexOf(end, y, cpr);

                if (y == -1) return null;

                //if (y < beginPos + begin.Length + 1) continue;
                if (source[y - 1] != escapeChar) break;

                y++;
            }
        }
        else
        {
            y = source.IndexOf(end, beginPos + begin.Length, cpr);
        }

        if (beginPos == -1 || y == -1) return null;

        var startIndex                   = beginPos;
        if (!includeSearcher) startIndex += begin.Length;

        if (startIndex > y) return null;

        nextPos = y;
        if (includeSearcher) nextPos += end.Length;

        return startIndex == y ? string.Empty : source[startIndex..nextPos];
    }

    public static IEnumerable<Tuple<string, string>> BetweenAll(this string source, string begin1, string end1, string end2) =>
        BetweenAll(source, begin1, end1, end1, end2);

    public static List<Tuple<string, string>> BetweenAllList(this string value, string begin1, string end1, string begin2, string end2)
    {
        var tuples          = new List<Tuple<string, string>>();
        var currentStartPos = 0;

        for (var result = Between(value, begin1, end1, begin2, end2, ref currentStartPos);
             result != null;
             result = Between(value, begin1, end1, begin2, end2, ref currentStartPos))
            tuples.Add(result!);

        return tuples;
    }

    public static IEnumerable<Tuple<string, string>> BetweenAll(this string source, string begin1, string end1, string begin2, string end2)
    {
        if (string.IsNullOrEmpty(source))
            throw new ArgumentNullException(nameof(source), "Chuỗi nguồn null hoặc empty.");

        var currentStartPos = 0;

        for (var result = Between(source, begin1, end1, begin2, end2, ref currentStartPos);
             result != null;
             result = Between(source, begin1, end1, begin2, end2, ref currentStartPos))
            yield return result!;
    }

    public static Tuple<string?, string?>? Between(this string source, string begin1, string end1, string end2)
    {
        var nextPos = 0;

        return Between(source, begin1, end1, end2, ref nextPos);
    }

    public static Tuple<string?, string?>? Between(this string source, string begin1, string end1, string end2, ref int nextPos) =>
        Between(source, begin1, end1, end1, end2, ref nextPos);

    public static Tuple<string?, string?>? Between(this string source, string begin1, string end1, string begin2, string end2, ref int nextPos)
    {
        if (string.IsNullOrEmpty(source))
            throw new ArgumentNullException(nameof(source), "Chuỗi nguồn null hoặc empty.");

        var str1 = source.Between(begin1, end1, ref nextPos);
        var str2 = source.Between(begin2, end2, ref nextPos);

        if (str1.IsNullOrWhiteSpace() && str2.IsNullOrWhiteSpace()) return null;

        return new Tuple<string?, string?>(str1, str2);
    }

    public static Dictionary<string, string?> BetweenAllDic(this string source, string begin, string middle, string end)
    {
        if (string.IsNullOrEmpty(source))
            throw new ArgumentNullException(nameof(source), "Chuỗi nguồn null hoặc empty.");

        var dictionary      = new Dictionary<string, string?>();
        var currentStartPos = 0;

        for (var result = Between(source, begin, middle, end, ref currentStartPos);
             result != null;
             result = Between(source, begin, middle, end, ref currentStartPos))
            if (result.Item1 != null)
                dictionary.Add(result.Item1, result.Item2);

        return dictionary;
    }

    public static Dictionary<string, string?> BetweenAllDic(this string source, string begin1, string end1, string begin2, string end2)
    {
        if (string.IsNullOrEmpty(source))
            throw new ArgumentNullException(nameof(source), "Chuỗi nguồn null hoặc empty.");

        var dictionary      = new Dictionary<string, string?>();
        var currentStartPos = 0;

        for (var result = Between(source, begin1, end1, begin2, end2, ref currentStartPos);
             result != null;
             result = Between(source, begin1, end1, begin2, end2, ref currentStartPos))
            if (result.Item1 != null)
                dictionary.Add(result.Item1, result.Item2);

        return dictionary;
    }

    public static NameValueCollection BetweenAllNvc(this string source, string begin1, string end1, string begin2, string end2)
    {
        if (string.IsNullOrEmpty(source))
            throw new ArgumentNullException(nameof(source), "Chuỗi nguồn null hoặc empty.");

        var collection      = new NameValueCollection();
        var currentStartPos = 0;

        for (var result = Between(source, begin1, end1, begin2, end2, ref currentStartPos);
             result != null;
             result = Between(source, begin1, end1, begin2, end2, ref currentStartPos))
            collection.Add(result.Item1, result.Item2);

        return collection;
    }

    #endregion
}