namespace UrlShortener.Presentation.Endpoints;

using Application.Common.Exceptions;
using Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public static class RedirectEndpoint
{
    public static WebApplication MapRedirectEndpoint(this WebApplication app)
    {
        _ = app.MapGet(
                "/{shortCode}",
                async ([Validate] string shortCode, [FromServices] ISender sender) =>
                {
                    try
                    {
                        var url = await sender.Send(
                            new GetOriginalUrlQuery { ShortCode = shortCode }
                        );
                        return Results.Redirect(url); // 302
                    }
                    catch (NotFoundException ex)
                    {
                        return TypedResults.NotFound(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        return TypedResults.Problem(
                            ex.StackTrace,
                            ex.Message,
                            StatusCodes.Status500InternalServerError
                        );
                    }
                }
            )
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .Produces(302)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("url-redirect")
            .WithSummary("Redirects to original URL")
            .WithDescription("Redirects to original URL with HTTP 302 code by short code.");

        return app;
    }
}
