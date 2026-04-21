namespace UrlShortener.Infrastructure.Databases.UrlShortener.Configuration;

using Application.Urls.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class UrlConfiguration : EntityConfiguration<Url>
{
    public override void Configure(EntityTypeBuilder<Url> builder)
    {
        builder.HasIndex(x => x.ShortCode).IsUnique();
    }
}
