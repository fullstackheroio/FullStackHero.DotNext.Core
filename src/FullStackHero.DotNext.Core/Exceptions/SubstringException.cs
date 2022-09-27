namespace FullStackHero.DotNext.Core.Exceptions;

/// <inheritdoc />
/// <summary>
///     An exception is that one or more substrings between two substrings could not be found.
/// </summary>
public class SubstringException : Exception
{
    /// <inheritdoc />
    /// <summary>
    ///     An exception is that one or more substrings between two substrings could not be found.
    /// </summary>
    public SubstringException()
    {
    }

    /// <inheritdoc />
    /// <inheritdoc cref="SubstringException()" />
    public SubstringException(string message) : base(message)
    {
    }

    /// <inheritdoc />
    /// <inheritdoc cref="SubstringException()" />
    public SubstringException(string message, Exception innerException) : base(message, innerException)
    {
    }
}