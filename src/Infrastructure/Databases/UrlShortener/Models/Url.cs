namespace UrlShortener.Infrastructure.Databases.UrlShortener.Models;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal record Url : Entity<int>
{
    public string ShortCode { get; set; }

    public string OriginalUrl { get; init; }
}
