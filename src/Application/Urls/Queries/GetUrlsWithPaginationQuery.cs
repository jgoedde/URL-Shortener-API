namespace UrlShortener;

using Application;
using Application.Urls.Entities;
using Application.Users;
using Infrastructure.Databases.UrlShortener.Models;
using MediatR;

public record GetUrlsWithPaginationQuery(int PageNumber, int PageSize)
    : IRequest<PaginatedList<Url>>;

internal sealed class GetUrlsHandler(IApplicationDbContext dbContext, ICurrentUser currentUser)
    : IRequestHandler<GetUrlsWithPaginationQuery, PaginatedList<Url>>
{
    public async Task<PaginatedList<Url>> Handle(
        GetUrlsWithPaginationQuery request,
        CancellationToken cancellationToken
    )
    {
        return await dbContext
            .Urls.OrderByDescending(x => x.Created)
            .Where(it => it.CreatedBy.Id == currentUser.Id)
            .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
