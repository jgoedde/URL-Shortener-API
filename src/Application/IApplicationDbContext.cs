namespace UrlShortener.Application;

using Microsoft.EntityFrameworkCore;
using Urls.Entities;
using Users.Entities;

public interface IApplicationDbContext
{
    public DbSet<Url> Urls { get; }

    public DbSet<AppUser> Users { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
