using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Extensions.Configuration;
using Soenneker.Upstash.HttpClients.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.Upstash.HttpClients;

public sealed class UpstashOpenApiHttpClient : IUpstashOpenApiHttpClient
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _config;

    private const string _prodBaseUrl = "https://api.upstash.com/v2/";

    public UpstashOpenApiHttpClient(IHttpClientCache httpClientCache, IConfiguration config)
    {
        _httpClientCache = httpClientCache;
        _config = config;
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        return _httpClientCache.Get(nameof(UpstashOpenApiHttpClient), (config: _config, baseUrl: _config["Upstash:ClientBaseUrl"] ?? _prodBaseUrl), static state =>
        {
            var email = state.config.GetValueStrict<string>("Upstash:Email");
            var apiKey = state.config.GetValueStrict<string>("Upstash:ApiKey");
            if (string.IsNullOrWhiteSpace(email) || email.Contains(':') || string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("Upstash:Email and Upstash:ApiKey must be nonempty; the email cannot contain a colon.");

            var baseUri = new Uri(state.baseUrl.TrimEnd('/') + "/", UriKind.Absolute);
            if (baseUri.Scheme != Uri.UriSchemeHttps || !string.IsNullOrEmpty(baseUri.UserInfo) ||
                !string.IsNullOrEmpty(baseUri.Query) || !string.IsNullOrEmpty(baseUri.Fragment))
                throw new InvalidOperationException("Upstash:ClientBaseUrl must be an HTTPS URL without credentials, query, or fragment.");

            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{email}:{apiKey}"));

            return new HttpClientOptions
            {
                BaseAddress = baseUri,
                DefaultRequestHeaders = new Dictionary<string, string>
                {
                    {"Authorization", $"Basic {credentials}"},
                    {"Accept", "application/json"},
                }
            };
        }, cancellationToken);
    }

    public void Dispose()
    {
        _httpClientCache.RemoveSync(nameof(UpstashOpenApiHttpClient));
    }

    public ValueTask DisposeAsync()
    {
        return _httpClientCache.Remove(nameof(UpstashOpenApiHttpClient));
    }
}
