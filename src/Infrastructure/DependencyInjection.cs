namespace UrlShortener.Infrastructure;

using Application;
using Application.Users;
using Application.Users.Entities;
using Databases.UrlShortener;
using Databases.UrlShortener.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users;

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

        _ = services
            .AddIdentityApiEndpoints<AppUser>(options =>
            {
                options.Password = new PasswordOptions
                {
                    RequireDigit = false,
                    RequiredLength = 2,
                    RequireLowercase = false,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false,
                };
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        _ = services.AddAuthorization();

        _ = services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>()
        );

        _ = services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        _ = services.AddSingleton(TimeProvider.System);

        _ = services.AddHttpContextAccessor();
        _ = services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}
