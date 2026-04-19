namespace UrlShortener.Infrastructure;

using System;
using Application.Authors;
using Application.Movies;
using Application.Reviews;
using Databases.MovieReviews;
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
        _ = services.AddDbContext<MovieReviewsDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("Default"))
        );

        _ = services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        _ = services.AddScoped<EntityFrameworkMovieReviewsRepository>();

        _ = services.AddScoped<IAuthorsRepository>(p =>
            p.GetRequiredService<EntityFrameworkMovieReviewsRepository>()
        );
        _ = services.AddScoped<IMoviesRepository>(x =>
            x.GetRequiredService<EntityFrameworkMovieReviewsRepository>()
        );
        _ = services.AddScoped<IReviewsRepository>(x =>
            x.GetRequiredService<EntityFrameworkMovieReviewsRepository>()
        );

        _ = services.AddSingleton(TimeProvider.System);

        return services;
    }
}
