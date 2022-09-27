namespace FullStackHero.DotNext.Core.Http;

/// <summary>
///     https://github.com/mboukhlouf/WebProxyService
/// </summary>
public class WebProxyService : IWebProxy
{
    #region Private fields

    private IWebProxy? _proxy;

    #endregion

    #region Properties

    public IWebProxy? Proxy
    {
        get => _proxy ??= WebRequest.DefaultWebProxy;
        set => _proxy = value;
    }

    #endregion

    #region Constructor

    public WebProxyService()
    {
    }

    public WebProxyService(IWebProxy? proxy) => Proxy = proxy;

    #endregion

    #region Implementation of IWebProxy

    /// <summary>Returns the URI of a proxy.</summary>
    /// <param name="destination">A <see cref="T:System.Uri" /> that specifies the requested Internet resource.</param>
    /// <returns>
    ///     A <see cref="T:System.Uri" /> instance that contains the URI of the proxy used to contact
    ///     <paramref name="destination" />.
    /// </returns>
    public Uri? GetProxy(Uri destination) => Proxy?.GetProxy(destination);

    /// <summary>Indicates that the proxy should not be used for the specified host.</summary>
    /// <param name="host">The <see cref="T:System.Uri" /> of the host to check for proxy use.</param>
    /// <returns>
    ///     <see langword="true" /> if the proxy server should not be used for <paramref name="host" />; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public bool IsBypassed(Uri host) => Proxy is not null && Proxy.IsBypassed(host);

    /// <summary>The credentials to submit to the proxy server for authentication.</summary>
    /// <returns>
    ///     An <see cref="T:System.Net.ICredentials" /> instance that contains the credentials that are needed to
    ///     authenticate a request to the proxy server.
    /// </returns>
    public ICredentials? Credentials
    {
        get => Proxy?.Credentials;
        set
        {
            if (Proxy is not null) Proxy.Credentials = value;
        }
    }

    #endregion
}