namespace UrlShortener.Application;

using Microsoft.EntityFrameworkCore;
using Urls.Entities;

public interface IApplicationDbContext
{
    public DbSet<Url> Urls { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
