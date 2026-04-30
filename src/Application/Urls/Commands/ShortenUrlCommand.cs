namespace UrlShortener;

using Application;
using Application.Common.Enums;
using Application.Common.Exceptions;
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

        NotFoundException.ThrowIfNull(user, EntityType.User);

        // TODO: Get `nextId` via DB instead of doing 2 UOW commits.

        var entity = new Url
        {
            Id = 287393928,
            ShortCode = "PLACEHOLDER",
            OriginalUrl = request.LongUrl,
            CreatedBy = user!,
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
