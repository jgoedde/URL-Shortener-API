namespace UrlShortener.Application.Urls.Entities;

using Infrastructure.Databases.UrlShortener.Models;
using Users.Entities;

public class Url : BaseAuditableEntity
{
    public string ShortCode { get; set; }

    public string OriginalUrl { get; init; }

    public AppUser CreatedBy { get; init; }
}
