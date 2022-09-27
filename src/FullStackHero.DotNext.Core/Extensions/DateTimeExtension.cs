namespace FullStackHero.DotNext.Core.Extensions;

public static class DateTimeExtension
{
    #region Private fields

    private static readonly string[] DateTimeFormats =
    {
        "yyyy",
        "d/M/yy",
        "d/M/yyyy",
        "d/MM/yy",
        "d/MM/yyyy",
        "dd/M/yy",
        "dd/M/yyyy",
        "dd/MM/yy",
        "dd/MM/yyyy",
        "M/yyyy",
        "MM/yyyy",
        "d-M-yy",
        "d-M-yyyy",
        "d-MM-yy",
        "d-MM-yyyy",
        "dd-M-yy",
        "dd-M-yyyy",
        "dd-MM-yy",
        "dd-MM-yyyy",
        "M-yyyy",
        "MM-yyyy",
        "M/d/yyyy h:mm:ss tt",
        "M/d/yyyy h:mm tt",
        "MM/dd/yyyy hh:mm:ss",
        "M/d/yyyy h:mm:ss",
        "M/d/yyyy hh:mm tt",
        "M/d/yyyy hh tt",
        "M/d/yyyy h:mm",
        "M/d/yyyy h:mm",
        "MM/dd/yyyy hh:mm",
        "M/dd/yyyy hh:mm",
        "MM/d/yyyy HH:mm:ss.ffffff",
        "yyyy-MM-ddTHH:mm:ss",    // eg 1997-07-16T19:20:30
        "yyyy-MM-ddThh:mm:ssTZD", // eg 1997-07-16T19:20:30+01:00
        "yyyy-MM-ddThh:mmZ",      // eg 1997-07-16T19:20Z
        "yyyy-MM-ddTHH:mm:sszzz"  // eg 1988-07-27T00:00:00+07:00
    };

    #endregion

    public static string[] DateVietNamFormats =
    {
        "yyyy",
        "d/M/yy",
        "d/M/yyyy",
        "d/MM/yy",
        "d/MM/yyyy",
        "dd/M/yy",
        "dd/M/yyyy",
        "dd/MM/yy",
        "dd/MM/yyyy",
        "M/yyyy",
        "MM/yyyy",
        "d-M-yy",
        "d-M-yyyy",
        "d-MM-yy",
        "d-MM-yyyy",
        "dd-M-yy",
        "dd-M-yyyy",
        "dd-MM-yy",
        "dd-MM-yyyy",
        "M-yyyy",
        "MM-yyyy",
        "d.M.yy",
        "d.M.yyyy",
        "d.MM.yy",
        "d.MM.yyyy",
        "dd.M.yy",
        "dd.M.yyyy",
        "dd.MM.yy",
        "dd.MM.yyyy",
        "M.yyyy",
        "MM.yyyy"
    };

    /// <summary>
    ///     Convert the string to DateTime object.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="dateTime"></param>
    /// <param name="formats"></param>
    /// <returns></returns>
    public static bool ToDate(this string? input, out DateTime? dateTime, string?[]? formats = null)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            dateTime = null;

            return false;
        }

        if (DateTime.TryParseExact(input, formats ?? DateTimeFormats, new CultureInfo("en-US"), DateTimeStyles.None, out var result))
        {
            dateTime = result;

            return true;
        }

        dateTime = null;

        return false;
    }

    /// <summary>
    ///     Convert the string to date string with formats "dd/MM/yyyy"
    /// </summary>
    /// <param name="input"></param>
    /// <param name="format"></param>
    /// <param name="formats"></param>
    /// <returns></returns>
    public static string? ToDateString(this string? input, string format = "dd/MM/yyyy", string?[]? formats = null) => ToDate(input, out var dateTime, formats) ? dateTime?.ToString(format) : null;
}