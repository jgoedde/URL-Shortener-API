namespace UrlShortener.Infrastructure;

using Application;
using Databases.UrlShortener;
using Databases.UrlShortener.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        _ = services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        _ = services.AddDbContext<ApplicationDbContext>(
            (sp, opt) =>
            {
                opt.UseNpgsql(configuration.GetConnectionString("Default"));
                opt.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            }
        );

        _ = services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>()
        );

        _ = services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        _ = services.AddSingleton(TimeProvider.System);

        return services;
    }
}
