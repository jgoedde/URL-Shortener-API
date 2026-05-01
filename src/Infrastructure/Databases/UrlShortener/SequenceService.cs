namespace UrlShortener.Infrastructure.Databases.UrlShortener;

using Application.Urls;
using Microsoft.EntityFrameworkCore;

internal sealed class SequenceService(ApplicationDbContext dbContext) : ISequenceService
{
    public async Task<int> NextUrlIdAsync(CancellationToken cancellationToken)
    {
        return await dbContext
            .Database.SqlQuery<int>($"SELECT nextval('urls_id_seq') AS \"Value\"")
            .FirstOrDefaultAsync(cancellationToken);
    }
}
