namespace UrlShortener.Infrastructure.Databases.UrlShortener;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Models;

internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Author> Authors { get; set; }

    public DbSet<Movie> Movies { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DbSet<Url> Urls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
