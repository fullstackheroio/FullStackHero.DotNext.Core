namespace FullStackHero.DotNext.Core.Extensions;

public static class EnumerableExtensions
{
    private static readonly Random Rnd = new();

    public static T? Random<T>(this IEnumerable<T> list) => list.ToArray().Random();

    public static T? Random<T>(this T?[] array) => array.Length <= 0 ? default : array[Rnd.Next(0, array.Length)];
}