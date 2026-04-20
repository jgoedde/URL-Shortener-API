namespace UrlShortener.Infrastructure.Databases.UrlShortener;

using System.Reflection;
using Application;
using Application.Urls.Entities;
using Microsoft.EntityFrameworkCore;

internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options),
        IApplicationDbContext
{
    public DbSet<Url> Urls => this.Set<Url>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
