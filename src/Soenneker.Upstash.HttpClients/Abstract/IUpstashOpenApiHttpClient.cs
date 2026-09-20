using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Upstash.HttpClients.Abstract;

/// <summary>
/// Provides a cached HTTP client for the Upstash Developer API using HTTP Basic authentication.
/// </summary>
public interface IUpstashOpenApiHttpClient: IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the shared client configured with Upstash:Email and Upstash:ApiKey.
    /// Upstash:ClientBaseUrl optionally overrides the default https://api.upstash.com/v2/ endpoint.
    /// Callers must not dispose the returned client; its lifetime is managed by the cache.
    /// </summary>
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);
}
