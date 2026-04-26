namespace UrlShortener;

using Application;
using Application.Urls.Entities;
using Infrastructure.Databases.UrlShortener.Models;
using MediatR;

public record GetUrlsWithPaginationQuery(int PageNumber, int PageSize)
    : IRequest<PaginatedList<Url>>;

public class GetUrlsHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetUrlsWithPaginationQuery, PaginatedList<Url>>
{
    public async Task<PaginatedList<Url>> Handle(
        GetUrlsWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        return await dbContext
            .Urls.OrderByDescending(x => x.Created)
            .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
