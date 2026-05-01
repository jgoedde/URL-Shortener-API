namespace UrlShortener.Infrastructure.Databases.UrlShortener;

using System.Reflection;
using Application;
using Application.Urls.Entities;
using Application.Users.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<AppUser>(options),
        IApplicationDbContext
{
    public DbSet<Url> Urls => this.Set<Url>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.HasSequence<int>("urls_id_seq");

        _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
