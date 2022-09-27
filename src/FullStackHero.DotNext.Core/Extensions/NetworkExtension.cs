namespace FullStackHero.DotNext.Core.Extensions;

public static class NetworkExtension
{
    /// <summary>
    ///     Kiểm tra kết nối mạng khả dụng
    /// </summary>
    /// <param name="hostNameOrAddress">IP/Host address: 192.168.1.1 or google.com</param>
    /// <param name="timeOut">Ping timeout. Mặc định: 1000 ms</param>
    /// <returns></returns>
    public static async Task<bool> SendPingAsync(this string hostNameOrAddress, int timeOut = 1000)
    {
        if (string.IsNullOrWhiteSpace(hostNameOrAddress))
            throw new ArgumentNullException(nameof(hostNameOrAddress), "Host name hoặc IPnull hoặc empty.");

        if (hostNameOrAddress.Length > 255)
            throw new ArgumentOutOfRangeException(nameof(hostNameOrAddress), "Chiều dài của host name hoặc IP lớn hơn 255.");

        if (timeOut < 0)
            throw new ArgumentOutOfRangeException(nameof(timeOut), "Timeout nhỏ hơn < 0.");

        try
        {
            using var ping  = new Ping();
            var       reply = await ping.SendPingAsync(hostNameOrAddress, timeOut).ConfigureAwait(false);

            return reply.Status switch
            {
                IPStatus.Success => true,
                _                => false
            };
        }
        catch (PingException)
        {
            return false;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    /// <summary>
    ///     Resolves a host name or IP address to an System.Net.IPHostEntry instance.
    /// </summary>
    /// <param name="hostNameOrAddress">The host name or IP address to resolve.</param>
    /// <param name="ipHostEntry"></param>
    /// <returns></returns>
    public static bool GetHostEntry(this string hostNameOrAddress, out IPHostEntry? ipHostEntry)
    {
        if (string.IsNullOrWhiteSpace(hostNameOrAddress))
            throw new ArgumentNullException(nameof(hostNameOrAddress), "Host name hoặc IP null hoặc empty.");

        if (hostNameOrAddress.Length > 255)
            throw new ArgumentOutOfRangeException(nameof(hostNameOrAddress), "Chiều dài của host name hoặc IP lớn hơn 255.");

        try
        {
            ipHostEntry = Dns.GetHostEntry(hostNameOrAddress);

            return ipHostEntry.AddressList.Any();
        }
        catch (SocketException)
        {
            // An error was encountered when resolving the hostNameOrAddress parameter.
            ipHostEntry = null;

            return false;
        }
        catch (ArgumentException)
        {
            // The hostNameOrAddress parameter is an invalid IP address.
            throw new ArgumentException("Địa chỉ IP không hợp lệ.", nameof(hostNameOrAddress));
        }
    }

    public static async Task<IPHostEntry?> GetHostEntryAsync(this string hostNameOrAddress)
    {
        if (string.IsNullOrWhiteSpace(hostNameOrAddress))
            throw new ArgumentNullException(nameof(hostNameOrAddress), "Host name hoặc IP null hoặc empty.");

        if (hostNameOrAddress.Length > 255)
            throw new ArgumentOutOfRangeException(nameof(hostNameOrAddress), "Chiều dài của host name hoặc IP lớn hơn 255.");

        try
        {
            return await Dns.GetHostEntryAsync(hostNameOrAddress).ConfigureAwait(false);
        }
        catch (SocketException)
        {
            // An error was encountered when resolving the hostNameOrAddress parameter.
            return null;
        }
        catch (ArgumentException)
        {
            // The hostNameOrAddress parameter is an invalid IP address.
            throw new ArgumentException("Địa chỉ IP không hợp lệ.", nameof(hostNameOrAddress));
        }
    }
}