# Soenneker.Upstash.HttpClients

A cached HTTP client for the Upstash Developer API (.NET 10).

## Configuration

Set `Upstash:Email` and `Upstash:ApiKey` in your application's secret configuration (environment variables: `Upstash__Email` and `Upstash__ApiKey`). The client encodes the credentials and sends HTTP Basic authentication. Use a native Upstash account's Developer API key, not a Redis REST token.

`Upstash:ClientBaseUrl` optionally overrides `https://api.upstash.com/v2/`. It must be an HTTPS URL without credentials, query, or fragment. Relative paths preserve the `/v2/` prefix.

```csharp
using Soenneker.Upstash.HttpClients.Abstract;
using Soenneker.Upstash.HttpClients.Registrars;

services.AddUpstashOpenApiHttpClientAsSingleton();
// Resolve IUpstashOpenApiHttpClient through dependency injection.
HttpClient client = await upstashHttpClient.Get(cancellationToken);
using HttpResponseMessage response = await client.GetAsync("redis/databases", cancellationToken);
response.EnsureSuccessStatusCode();
```

The cache owns the returned HttpClient; do not dispose it. Scoped registration is also available via `AddUpstashOpenApiHttpClientAsScoped()`.

[Upstash authentication](https://upstash.com/docs/devops/developer-api/authentication)
