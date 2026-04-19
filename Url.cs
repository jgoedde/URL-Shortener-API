namespace UrlShortener;

public record Url : Entity
{
    /// <summary>
    /// Base62 of Id for public exposure.
    /// Used to resolve the URL.
    /// </summary>
    public string ShortCode { get; init; }

    public string LongUrl { get; init; }
}
