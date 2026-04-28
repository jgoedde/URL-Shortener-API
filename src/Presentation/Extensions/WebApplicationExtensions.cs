namespace UrlShortener.Presentation.Extensions;

using System.Diagnostics.CodeAnalysis;
using Application.Users.Entities;
using Endpoints;
using Serilog;

[ExcludeFromCodeCoverage]
public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        #region Logging

        _ = app.UseHttpLogging();
        _ = app.UseSerilogRequestLogging();

        #endregion Logging

        #region Security

        _ = app.UseHsts();
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        #endregion Security

        #region API Configuration

        _ = app.UseHttpsRedirection();

        #endregion API Configuration

        #region Swagger

        _ = app.MapOpenApi();
        _ = app.UseSwaggerUI(options =>
            options.SwaggerEndpoint("/openapi/v1.json", "UrlShortener API v1")
        );

        #endregion Swagger

        #region MinimalApi

        _ = app.MapVersionEndpoints();
        _ = app.MapRedirectEndpoint();
        _ = app.MapUrlEndpoints();
        _ = app.MapIdentityApi<AppUser>().WithTags("auth");

        #endregion MinimalApi

        return app;
    }
}
