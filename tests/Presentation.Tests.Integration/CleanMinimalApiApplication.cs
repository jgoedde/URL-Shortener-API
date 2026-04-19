namespace UrlShortener.Presentation.Tests.Integration;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

internal sealed class UrlShortenerApplication(string environment = "Development")
    : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        _ = builder.UseEnvironment(environment);

        return base.CreateHost(builder);
    }
}
