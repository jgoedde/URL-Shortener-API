namespace UrlShortener.Infrastructure.Databases.UrlShortener.Models;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal record Author : Entity<Guid>
{
    public string FirstName { get; init; }

    public string LastName { get; init; }

    public ICollection<Review> Reviews { get; init; }
}
