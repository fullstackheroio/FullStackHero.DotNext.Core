namespace FullStackHero.DotNext.Core.Extensions;

public static class RandomExtension
{
    /// <summary>
    ///     Phương thức lấy ngẫu nhiên một số có kiểu giá trị double.
    /// </summary>
    /// <param name="random"></param>
    /// <param name="minimum"></param>
    /// <param name="maximum"></param>
    /// <returns></returns>
    public static double NextDouble(this Random random, double minimum, double maximum)
    {
        if (minimum >= maximum)
            throw new ArgumentOutOfRangeException(nameof(minimum), "minimum greater or equal maximum");

        return random.NextDouble() * (maximum - minimum) + minimum;
    }
}