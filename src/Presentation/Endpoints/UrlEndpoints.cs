namespace UrlShortener.Presentation.Endpoints;

using Application.Common;
using Application.Urls.Commands;
using Application.Urls.Queries;
using Filters;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Requests;
using Responses;

public static class UrlEndpoints
{
    public static WebApplication MapUrlEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/urls")
            .RequireAuthorization()
            .WithTags("urls")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithDescription("Lookup, Find and Manipulate Urls");

        _ = root.MapPost("/", CreateUrl)
            .Produces<UrlApiResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem()
            .WithSummary("Shorten an URL");

        _ = root.MapGet("/", GetUrlsWithPagination)
            .Produces<PaginatedList<UrlApiResponse>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Gets a list of URLs ordered by their creation time.");

        return app;
    }

    private static async Task<Results<Created<UrlApiResponse>, ProblemHttpResult>> CreateUrl(
        [Validate] [FromBody] ShortenUrlRequest request,
        [FromServices] ISender sender,
        [FromServices] LinkGenerator linker,
        HttpContext ctx
    )
    {
        try
        {
            var url = await sender.Send(new ShortenUrlCommand(request.Url));

            return TypedResults.Created(
                $"/api/urls/{url.ShortCode}",
                new UrlApiResponse(
                    ShortUrl: linker.GetUriByName(ctx, "url-redirect", new { url.ShortCode })
                        ?? string.Empty,
                    OriginalUrl: url.OriginalUrl,
                    ShortCode: url.ShortCode,
                    Created: url.Created,
                    LastModified: url.LastModified
                )
            );
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

    private static async Task<
        Results<Ok<PaginatedList<UrlApiResponse>>, ProblemHttpResult>
    > GetUrlsWithPagination(
        [Validate] [AsParameters] GetUrlsRequest request,
        [FromServices] ISender sender,
        [FromServices] LinkGenerator linker,
        HttpContext ctx
    )
    {
        try
        {
            var list = await sender.Send(
                new GetUrlsWithPaginationQuery(request.PageNumber, request.PageSize)
            );

            var mapped = list.Map(url => new UrlApiResponse(
                ShortUrl: linker.GetUriByName(ctx, "url-redirect", new { url.ShortCode })
                    ?? string.Empty,
                OriginalUrl: url.OriginalUrl,
                ShortCode: url.ShortCode,
                Created: url.Created,
                LastModified: url.LastModified
            ));

            return TypedResults.Ok(mapped);
        }
        catch (Exception e)
        {
            return TypedResults.Problem(
                e.StackTrace,
                e.Message,
                StatusCodes.Status500InternalServerError
            );
        }
    }
}
