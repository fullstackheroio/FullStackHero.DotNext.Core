namespace FullStackHero.DotNext.Core.Extensions;

public static class PhoneExtension
{
    #region Static methods

    /// <summary>
    ///     Convert a string to msisdn start with number 84
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string? ToPhoneWithPrefix84(this string? input)
    {
        input = input.KeepDigit();

        if (string.IsNullOrEmpty(input)) return input;

        return input.Length switch
        {
            9  => Phone9CharsRegex.IsMatch(input) ? $"84{input}" : string.Empty,
            10 => Phone10CharsRegex.IsMatch(input) ? $"84{input[1..]}" : string.Empty,
            11 => Phone11CharsRegex.IsMatch(input) ? input : string.Empty,
            _  => string.Empty
        };
    }

    /// <summary>
    ///     Convert a string to msisdn start with number 0
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string? ToPhone(this string? input)
    {
        input = input.KeepDigit();

        if (string.IsNullOrEmpty(input)) return input;

        return input.Length switch
        {
            9  => Phone9CharsRegex.IsMatch(input) ? $"0{input}" : string.Empty,
            10 => Phone10CharsRegex.IsMatch(input) ? input : string.Empty,
            11 => Phone11CharsRegex.IsMatch(input) ? $"0{input[2..]}" : string.Empty,
            _  => string.Empty
        };
    }

    /// <summary>
    ///     Convert a string to msisdn start without number 0
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string? ToPhoneWithoutStartZero(this string? input)
    {
        input = input.KeepDigit();

        if (string.IsNullOrEmpty(input)) return input;

        return input.Length switch
        {
            9  => Phone9CharsRegex.IsMatch(input) ? input : string.Empty,
            10 => Phone10CharsRegex.IsMatch(input) ? input[1..] : string.Empty,
            11 => Phone11CharsRegex.IsMatch(input) ? input[2..] : string.Empty,
            _  => string.Empty
        };
    }

    /// <summary>
    ///     Chuyển một chuỗi sang định dạng số máy bàn cố định không bắt đầu với số 0.
    /// </summary>
    /// <param name="input">Số điện thoại bàn cần kiểm tra.</param>
    /// <returns></returns>
    public static string? ToFixPhoneWithoutStartZero(this string? input)
    {
        input = input.KeepDigit();

        if (string.IsNullOrEmpty(input)) return input;

        return input.Length switch
        {
            10 => FixPhone10CharsRegex.IsMatch(input) ? input : string.Empty,
            11 => FixPhone11CharsRegex.IsMatch(input) ? input[1..] : string.Empty,
            _  => string.Empty
        };
    }

    /// <summary>
    ///     Kiểm tra một chuỗi có phải là một số điện thoại di động hợp lệ.
    /// </summary>
    /// <param name="input">Chuỗi cần kiểm tra.</param>
    /// <param name="phoneNumber">Số điện thoại sau khi đã convert.</param>
    /// <returns>
    ///     + true: là định dạng số di động. PhoneNumber có đầu ra là định dạng số điện thoại di động bắt đầu với chữ số 0.
    ///     + false: không phải định dạng số di động. PhoneNumber có đầu ra là chuỗi Empty.
    /// </returns>
    public static bool IsPhoneNumber(this string? input, out string? phoneNumber)
    {
        phoneNumber = input.ToPhone();

        return !string.IsNullOrEmpty(phoneNumber);
    }

    /// <summary>
    ///     Kiểm tra một chuỗi có phải là một số điện thoại cố định hợp lệ.
    /// </summary>
    /// <param name="input">Chuỗi cần kiểm tra.</param>
    /// <param name="phoneNumber">Số điện thoại sau khi đã convert.</param>
    /// <returns>
    ///     + true: là định dạng số điện thoại cố định không bắt đầu với chữ số 0.
    ///     + false: không phải định dạng số điện thoại cố định. PhoneNumber có đầu ra là chuỗi Empty.
    /// </returns>
    public static bool IsFixPhoneNumber(this string? input, out string? phoneNumber)
    {
        phoneNumber = input.ToFixPhoneWithoutStartZero();

        return !string.IsNullOrEmpty(phoneNumber);
    }

    #endregion

    #region Private fields

    private static readonly Regex Phone9CharsRegex     = new(@"(9|8|7|5|3)([0-9]{8})$", RegexOptions.Compiled);
    private static readonly Regex Phone10CharsRegex    = new(@"0(9|8|7|5|3)([0-9]{8})$", RegexOptions.Compiled);
    private static readonly Regex Phone11CharsRegex    = new(@"84(9|8|7|5|3)([0-9]{8})$", RegexOptions.Compiled);
    private static readonly Regex FixPhone10CharsRegex = new(@"(2)([0-9]{9})$", RegexOptions.Compiled);
    private static readonly Regex FixPhone11CharsRegex = new(@"(02)([0-9]{9})$", RegexOptions.Compiled);

    #endregion
}