using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Soenneker.TestHosts.Unit;
using System.Collections.Generic;
using Soenneker.Upstash.HttpClients.Registrars;

namespace Soenneker.Upstash.HttpClients.Tests;

public sealed class Host : UnitTestHost
{
    public override Task InitializeAsync()
    {
        SetupIoC(Services);

        return base.InitializeAsync();
    }

    private static void SetupIoC(IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.AddSerilog(dispose: false);
        });

        IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Upstash:Email"] = "test@example.com",
            ["Upstash:ApiKey"] = "test-key:with-colon",
            ["Upstash:ClientBaseUrl"] = "https://upstash.example.test/v2"
        }).Build();
        services.AddSingleton(config);

        services.AddUpstashOpenApiHttpClientAsScoped();
    }
}
