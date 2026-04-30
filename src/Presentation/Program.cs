using Serilog;
using UrlShortener.Infrastructure;
using UrlShortener.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args).ConfigureApplicationBuilder();

var app = builder.Build().ConfigureApplication();

await app.Services.EnsureDatabaseCreatedAsync();

try
{
    Log.Information("Starting host");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
