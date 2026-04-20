namespace UrlShortener.Infrastructure;

using System;
using Application.Authors;
using Application.Movies;
using Application.Reviews;
using Application.Urls;
using Databases.UrlShortener;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        _ = services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("Default"))
        );

        _ = services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        _ = services.AddScoped<EntityFrameworkDefaultRepository>();

        _ = services.AddScoped<IAuthorsRepository>(p =>
            p.GetRequiredService<EntityFrameworkDefaultRepository>()
        );
        _ = services.AddScoped<IMoviesRepository>(x =>
            x.GetRequiredService<EntityFrameworkDefaultRepository>()
        );
        _ = services.AddScoped<IReviewsRepository>(x =>
            x.GetRequiredService<EntityFrameworkDefaultRepository>()
        );
        _ = services.AddScoped<IUrlsRepository>(x =>
            x.GetRequiredService<EntityFrameworkDefaultRepository>()
        );

        _ = services.AddSingleton(TimeProvider.System);

        return services;
    }
}
