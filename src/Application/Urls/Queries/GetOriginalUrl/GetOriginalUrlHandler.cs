namespace UrlShortener.Application.Urls.Queries.GetOriginalUrl;

using Common.Enums;
using Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetOriginalUrlHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetOriginalUrlQuery, string>
{
    // TODO: Add caching
    public async Task<string> Handle(
        GetOriginalUrlQuery request,
        CancellationToken cancellationToken
    )
    {
        var url = await dbContext
            .Urls.AsNoTracking()
            .Where(it => it.ShortCode == request.ShortCode)
            .FirstOrDefaultAsync(cancellationToken);

        NotFoundException.ThrowIfNull(url, EntityType.Url);

        return url.OriginalUrl;
    }
}
