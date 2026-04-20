namespace UrlShortener.Application.Urls.Queries.GetOriginalUrl;

using Common.Enums;
using Common.Exceptions;
using MediatR;

public class GetOriginalUrlHandler(IUrlsRepository urlsRepository)
    : IRequestHandler<GetOriginalUrlQuery, string>
{
    // TODO: Add caching
    public async Task<string> Handle(
        GetOriginalUrlQuery request,
        CancellationToken cancellationToken
    )
    {
        var url = await urlsRepository.GetByShortCode(request.ShortCode, cancellationToken);

        NotFoundException.ThrowIfNull(url, EntityType.Url);

        return url.OriginalUrl;
    }
}
