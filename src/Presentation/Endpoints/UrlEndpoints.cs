namespace UrlShortener.Presentation.Endpoints;

using Application.Urls.Commands.ShortenUrl;
using Filters;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Requests;
using Entities = Application.Urls.Entities;

public static class UrlEndpoints
{
    public static WebApplication MapUrlEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/urls")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithDescription("Lookup, Find and Manipulate Urls");

        _ = root.MapPost("/", CreateUrl)
            .Produces<Entities.Url>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem()
            .WithSummary("Shorten an URL");

        return app;
    }

    private static async Task<
        Results<Created<Entities.Url>, NotFound<string>, ProblemHttpResult>
    > CreateUrl([Validate] [FromBody] ShortenUrlRequest request, [FromServices] ISender sender)
    {
        try
        {
            var response = await sender.Send(new ShortenUrlCommand(request.Url));

            return TypedResults.Created($"/api/urls/{response.ShortCode}", response);
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
