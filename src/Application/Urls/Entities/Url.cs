namespace UrlShortener.Application.Urls.Entities;

using Infrastructure.Databases.UrlShortener.Models;

public class Url : BaseAuditableEntity
{
    public string ShortCode { get; set; }

    public string OriginalUrl { get; init; }
}
