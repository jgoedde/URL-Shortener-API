namespace UrlShortener.Application.Urls.Queries;

using Common.Caching;
using Common.Enums;
using Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetOriginalUrlQuery : ICachedQuery<string>
{
    public required string ShortCode { get; init; }

    public string Key => $"url:{this.ShortCode}";

    public TimeSpan? Expiration { get; init; }
}

public class GetOriginalUrlHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetOriginalUrlQuery, string>
{
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

        return url!.OriginalUrl;
    }
}
