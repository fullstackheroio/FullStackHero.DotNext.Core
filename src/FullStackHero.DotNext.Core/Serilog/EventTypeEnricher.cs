namespace FullStackHero.DotNext.Core.Serilog;

/// <summary>
///     <see>
///         <cref>https://blog.datalust.co/serilog-tutorial/</cref>
///     </see>
///     <seealso>
///         <cref>https://benfoster.io/blog/serilog-best-practices/</cref>
///     </seealso>
/// </summary>
public class EventTypeEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        ArgumentNullException.ThrowIfNull(logEvent);
        ArgumentNullException.ThrowIfNull(propertyFactory);

        var hash        = ComputeHash(logEvent.MessageTemplate.Text, SHA256.Create);
        var numericHash = BitConverter.ToUInt32(hash, 0);
        var eventType   = propertyFactory.CreateProperty("EventType", numericHash);
        logEvent.AddPropertyIfAbsent(eventType);
    }

    /// <summary>
    ///     Calculate the hash value of a string using the algorithm <see cref="SHA256" />, <see cref="MD5" />...
    /// </summary>
    /// <param name="input">String to be hashed.</param>
    /// <param name="hashAlgorithm">Algorithm used to hash.</param>
    /// <returns></returns>
    private static byte[] ComputeHash(string input, Func<HashAlgorithm> hashAlgorithm)
    {
        // Create a algorithm
        using var algorithm = hashAlgorithm.Invoke();

        // ComputeHash - returns byte array  
        var bytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

        return bytes;
    }
}