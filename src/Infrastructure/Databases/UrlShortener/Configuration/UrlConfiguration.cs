namespace UrlShortener.Infrastructure.Databases.UrlShortener.Configuration;

using Application.Urls.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class UrlConfiguration : EntityConfiguration<Url>
{
    public override void Configure(EntityTypeBuilder<Url> builder)
    {
        builder.Property(x => x.Id).HasDefaultValueSql("nextval('urls_id_seq')");
        builder.HasIndex(x => x.ShortCode).IsUnique();
    }
}
