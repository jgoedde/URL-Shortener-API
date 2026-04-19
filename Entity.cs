using System.Diagnostics.CodeAnalysis;

namespace UrlShortener;

[ExcludeFromCodeCoverage]
public abstract record Entity
{
    public int Id { get; init; }

    public DateTime DateCreated { get; init; }

    public DateTime DateModified { get; set; }
}
