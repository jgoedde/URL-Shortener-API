namespace UrlShortener.Application;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        _ = services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(QueryCachingPipelineBehaviour<,>));
        });

        return services;
    }
}
