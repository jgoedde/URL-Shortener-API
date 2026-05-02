namespace UrlShortener.Application.Urls.Entities;

using Infrastructure.Databases.UrlShortener.Models;
using Users.Entities;

public class Url : BaseAuditableEntity
{
    public required string ShortCode { get; set; }

    public required string OriginalUrl { get; init; }

    public required AppUser CreatedBy { get; init; }
}
