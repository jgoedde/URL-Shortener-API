namespace UrlShortener.Application.Urls.Entities;

public record Url(int Id, string OriginalUrl, string ShortCode);
