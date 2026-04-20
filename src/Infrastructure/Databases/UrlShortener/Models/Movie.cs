namespace UrlShortener.Infrastructure.Databases.UrlShortener.Models;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal record Movie : Entity<Guid>
{
    public string Title { get; init; }

    public ICollection<Review> Reviews { get; init; }
}
