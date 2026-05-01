namespace UrlShortener.Presentation.Endpoints;

using Application.Common.Exceptions;
using Application.Urls.Queries;
using Filters;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public static class RedirectEndpoint
{
    public static WebApplication MapRedirectEndpoint(this WebApplication app)
    {
        _ = app.MapGet("/{shortCode}", Redirect)
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .Produces(302)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("url-redirect")
            .WithTags("root")
            .WithSummary("Redirects to original URL")
            .WithDescription("Redirects to original URL with HTTP 302 code by short code.");

        return app;
    }

    private static async Task<
        Results<RedirectHttpResult, ProblemHttpResult, NotFound<string>>
    > Redirect([Validate] string shortCode, [FromServices] ISender sender)
    {
        try
        {
            var url = await sender.Send(new GetOriginalUrlQuery { ShortCode = shortCode });
            return TypedResults.Redirect(url); // 302
        }
        catch (NotFoundException ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status404NotFound);
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
}
