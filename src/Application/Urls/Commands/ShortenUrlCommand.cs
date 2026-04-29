namespace UrlShortener;

using Application;
using Application.Urls;
using Application.Urls.Entities;
using Application.Users;
using MediatR;

public record ShortenUrlCommand(string LongUrl) : IRequest<Url>;

internal sealed class ShortenUrlHandler(
    IApplicationDbContext dbContext,
    UrlEncoder urlEncoder,
    ICurrentUser currentUser
) : IRequestHandler<ShortenUrlCommand, Url>
{
    public async Task<Url> Handle(ShortenUrlCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync(
            [currentUser.Id],
            cancellationToken: cancellationToken
        );

        if (user is null)
        {
            throw new InvalidOperationException("User not found.");
        }

        var entity = new Url
        {
            ShortCode = "PLACEHOLDER",
            OriginalUrl = request.LongUrl,
            CreatedBy = user,
        };

        dbContext.Urls.Add(entity);

        // Do a save here so the `entity.Id` gets populated.
        await dbContext.SaveChangesAsync(cancellationToken);

        var shortCode = urlEncoder.GetShortCode(entity.Id);
        entity.ShortCode = shortCode;

        await dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
