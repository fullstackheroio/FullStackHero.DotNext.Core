﻿namespace FullStackHero.DotNext.Core.Extensions;

public static class StreamExtension
{
    public static async Task CopyToAsync(this Stream source,
                                         Stream destination,
                                         int bufferSize,
                                         IProgress<long> progress = null!,
                                         CancellationToken cancellationToken = default)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        if (!source.CanRead)
            throw new ArgumentException("Readable", nameof(source));

        if (destination == null)
            throw new ArgumentNullException(nameof(destination));

        if (!destination.CanWrite)
            throw new ArgumentException("Writable", nameof(destination));

        if (bufferSize < 0)
            throw new ArgumentOutOfRangeException(nameof(bufferSize));

        var  buffer         = new byte[bufferSize];
        long totalBytesRead = 0;
        int  bytesRead;

        while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
        {
            await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
            totalBytesRead += bytesRead;
            progress?.Report(totalBytesRead);
        }
    }

    /// <summary>
    ///     Convert a stream to base64 string using MemoryStream.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static async Task<string> ToBase64Async(this Stream stream)
    {
        await using var ms = new MemoryStream();
        await stream.CopyToAsync(ms).ConfigureAwait(false);

        return Convert.ToBase64String(ms.ToArray());
    }

    /// <summary>
    ///     Convert a stream to base64 string using MemoryStream.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
    {
        await using var ms = new MemoryStream();
        await stream.CopyToAsync(ms).ConfigureAwait(false);

        return ms.ToArray();
    }
}