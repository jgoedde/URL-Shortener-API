namespace UrlShortener.Infrastructure.Databases.UrlShortener.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

internal class AuthorConfiguration : EntityConfiguration<Author, Guid>
{
    public override void Configure(EntityTypeBuilder<Author> builder)
    {
        base.Configure(builder);

        _ = builder
            .HasMany(m => m.Reviews)
            .WithOne(r => r.ReviewAuthor)
            .HasForeignKey(r => r.ReviewAuthorId);
    }
}
