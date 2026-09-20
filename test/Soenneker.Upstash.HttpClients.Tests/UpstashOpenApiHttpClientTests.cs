using System;
using System.Text;
using System.Threading.Tasks;
using Soenneker.Upstash.HttpClients.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Upstash.HttpClients.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class UpstashOpenApiHttpClientTests(Host host) : HostedUnitTest(host)
{
    [Test]
    public async Task Get_configures_basic_auth_and_preserves_base_path()
    {
        var client = await Resolve<IUpstashOpenApiHttpClient>(true).Get();
        await Assert.That(client.BaseAddress!.AbsoluteUri).IsEqualTo("https://upstash.example.test/v2/");
        await Assert.That(new Uri(client.BaseAddress, "redis/databases").AbsoluteUri)
            .IsEqualTo("https://upstash.example.test/v2/redis/databases");
        await Assert.That(client.DefaultRequestHeaders.Authorization!.Scheme).IsEqualTo("Basic");
        string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(client.DefaultRequestHeaders.Authorization.Parameter!));
        await Assert.That(decoded).IsEqualTo("test@example.com:test-key:with-colon");
    }

    [Test]
    public async Task Get_reuses_the_cached_client()
    {
        var wrapper = Resolve<IUpstashOpenApiHttpClient>(true);
        var first = await wrapper.Get();
        var second = await wrapper.Get();
        await Assert.That(ReferenceEquals(first, second)).IsTrue();
    }
}
