namespace UrlShortener.Application.Urls.Commands;

using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;
using Users;

public record ShortenUrlCommand(string LongUrl) : IRequest<Url>;

internal sealed class ShortenUrlHandler(
    IApplicationDbContext dbContext,
    ISequenceService sequenceService,
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

        var nextId = await sequenceService.NextUrlIdAsync(cancellationToken);
        var shortCode = UrlEncoder.GetShortCode(nextId);

        var entity = new Url
        {
            Id = nextId,
            ShortCode = shortCode,
            OriginalUrl = request.LongUrl,
            CreatedBy = user!,
        };

        dbContext.Urls.Add(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
