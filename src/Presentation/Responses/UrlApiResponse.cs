namespace UrlShortener.Presentation.Responses;

public record UrlApiResponse(
    string ShortUrl,
    string OriginalUrl,
    string ShortCode,
    DateTimeOffset Created,
    DateTimeOffset LastModified
);
