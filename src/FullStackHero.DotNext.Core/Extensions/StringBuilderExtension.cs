namespace FullStackHero.DotNext.Core.Extensions;

public static class StringBuilderExtension
{
    public static bool StartsWith(this StringBuilder stringBuilder, char prefix)
    {
        if (stringBuilder == null)
            throw new ArgumentNullException(nameof(stringBuilder));

        if (stringBuilder.Length == 0)
            return false;

        return stringBuilder[0] == prefix;
    }

    public static bool StartsWith(this StringBuilder stringBuilder, string prefix)
    {
        if (stringBuilder == null)
            throw new ArgumentNullException(nameof(stringBuilder));

        if (prefix == null)
            throw new ArgumentNullException(nameof(prefix));

        if (stringBuilder.Length < prefix.Length)
            return false;

        return !prefix.Where((t, i) => stringBuilder[i] != t).Any();
    }

    public static bool EndsWith(this StringBuilder stringBuilder, char suffix)
    {
        if (stringBuilder == null)
            throw new ArgumentNullException(nameof(stringBuilder));

        if (stringBuilder.Length == 0)
            return false;

        return stringBuilder[^1] == suffix;
    }

    public static bool EndsWith(this StringBuilder stringBuilder, string suffix)
    {
        if (stringBuilder == null)
            throw new ArgumentNullException(nameof(stringBuilder));

        if (suffix == null)
            throw new ArgumentNullException(nameof(suffix));

        if (stringBuilder.Length < suffix.Length)
            return false;

        return !suffix.Where((_, index) => stringBuilder[stringBuilder.Length - 1 - index] != suffix[suffix.Length - 1 - index]).Any();
    }

    public static void TrimStart(this StringBuilder stringBuilder, char trimChar)
    {
        if (stringBuilder == null)
            throw new ArgumentNullException(nameof(stringBuilder));

        for (var i = 0; i < stringBuilder.Length; i++)
        {
            if (stringBuilder[i] == trimChar)
                continue;

            if (i > 0) stringBuilder.Remove(0, i);

            return;
        }
    }

    public static void TrimEnd(this StringBuilder stringBuilder, char trimChar)
    {
        if (stringBuilder == null)
            throw new ArgumentNullException(nameof(stringBuilder));

        for (var i = stringBuilder.Length - 1; i >= 0; i--)
        {
            if (stringBuilder[i] == trimChar)
                continue;

            if (i != stringBuilder.Length - 1) stringBuilder.Remove(i + 1, stringBuilder.Length - i - 1);

            return;
        }
    }

    public static void Trim(this StringBuilder stringBuilder, char trimChar)
    {
        TrimEnd(stringBuilder, trimChar);
        TrimStart(stringBuilder, trimChar);
    }
}