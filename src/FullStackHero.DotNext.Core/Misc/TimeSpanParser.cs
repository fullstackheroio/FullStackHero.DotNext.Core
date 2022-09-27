namespace FullStackHero.DotNext.Core.Misc;

internal static class TimeSpanParser
{
    private const long MaxMillisInHour = 3600000L;

    public static string ToString(TimeSpan value)
    {
        var totalMilliseconds = (long)value.TotalMilliseconds;

        if (totalMilliseconds % MaxMillisInHour == 0L)
            return $"{totalMilliseconds / MaxMillisInHour}h";

        if (totalMilliseconds % 60000L == 0L && totalMilliseconds < MaxMillisInHour)
            return $"{totalMilliseconds / 60000L}m";

        if (totalMilliseconds % 1000L == 0L && totalMilliseconds < 60000L)
            return $"{totalMilliseconds / 1000L}s";

        return totalMilliseconds < 1000L ? $"{totalMilliseconds}ms" : value.ToString();
    }

    public static bool TryParse(string value, out TimeSpan result)
    {
        if (!string.IsNullOrEmpty(value))
        {
            value = value.ToLowerInvariant();
            var index = value.Length - 1;
            var num   = 1000;

            switch (value[index])
            {
                case 's' when value[index - 1] == 'm':
                    value = value[..^2];
                    num   = 1;

                    break;

                case 's':
                    value = value[..^1];
                    num   = 1000;

                    break;

                case 'm':
                    value = value[..^1];
                    num   = 60000;

                    break;

                case 'h':
                    value = value[..^1];
                    num   = 3600000;

                    break;

                default:
                {
                    if (value.IndexOf(':') != -1)
                        return TimeSpan.TryParse(value, out result);

                    break;
                }
            }

            if (double.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out var result1))
            {
                result = TimeSpan.FromMilliseconds(result1 * num);

                return true;
            }
        }

        result = new TimeSpan();

        return false;
    }
}