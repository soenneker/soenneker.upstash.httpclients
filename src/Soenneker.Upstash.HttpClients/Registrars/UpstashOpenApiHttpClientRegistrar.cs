using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Upstash.HttpClients.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.Upstash.HttpClients.Registrars;

/// <summary>
/// Registers the OpenAPI HttpClient wrapper for dependency injection.
/// </summary>
public static class UpstashOpenApiHttpClientRegistrar
{
    /// <summary>
    /// Adds <see cref="UpstashOpenApiHttpClient"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddUpstashOpenApiHttpClientAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
                .TryAddSingleton<IUpstashOpenApiHttpClient, UpstashOpenApiHttpClient>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="UpstashOpenApiHttpClient"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddUpstashOpenApiHttpClientAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
                .TryAddScoped<IUpstashOpenApiHttpClient, UpstashOpenApiHttpClient>();

        return services;
    }
}
