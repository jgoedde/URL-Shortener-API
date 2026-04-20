namespace UrlShortener.Presentation.Extensions;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Serilog;
using UrlShortener.Presentation.Endpoints;

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
        _ = app.MapAuthorEndpoints();
        _ = app.MapMovieEndpoints();
        _ = app.MapReviewEndpoints();
        _ = app.MapRedirectEndpoint();

        #endregion MinimalApi

        return app;
    }
}
