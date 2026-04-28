namespace UrlShortener;

using Application;
using Application.Urls;
using Application.Urls.Entities;
using MediatR;

public record ShortenUrlCommand(string LongUrl) : IRequest<Url>;

public class ShortenUrlHandler(IApplicationDbContext dbContext, UrlEncoder urlEncoder)
    : IRequestHandler<ShortenUrlCommand, Url>
{
    public async Task<Url> Handle(ShortenUrlCommand request, CancellationToken cancellationToken)
    {
        var entity = new Url { ShortCode = "PLACEHOLDER", OriginalUrl = request.LongUrl };

        dbContext.Urls.Add(entity);

        // Do a save here so the `entity.Id` gets populated.
        await dbContext.SaveChangesAsync(cancellationToken);

        var shortCode = urlEncoder.GetShortCode(entity.Id);
        entity.ShortCode = shortCode;

        await dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
