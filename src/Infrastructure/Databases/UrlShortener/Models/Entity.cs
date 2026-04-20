namespace UrlShortener.Infrastructure.Databases.UrlShortener.Models;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal abstract record Entity<TId>
{
    public TId Id { get; init; }

    public DateTime DateCreated { get; init; }

    public DateTime DateModified { get; set; }
}
