using Microsoft.EntityFrameworkCore;

namespace UrlShortener;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Url> Urls { get; set; }
}