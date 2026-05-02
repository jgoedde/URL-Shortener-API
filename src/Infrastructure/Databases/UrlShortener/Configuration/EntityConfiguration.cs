namespace UrlShortener.Infrastructure.Databases.UrlShortener.Configuration;

using Application.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T>
    where T : BaseAuditableEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        _ = builder.HasKey(e => e.Id);
        _ = builder.Property(m => m.Id).ValueGeneratedOnAdd().IsRequired();
        _ = builder.Property(m => m.Created).ValueGeneratedOnAdd().IsRequired();
        _ = builder.Property(m => m.LastModified).ValueGeneratedOnAdd().IsRequired();
    }
}
