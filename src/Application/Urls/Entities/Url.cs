namespace UrlShortener.Application.Urls.Entities;

using Common;
using Users.Entities;

public class Url : BaseAuditableEntity
{
    public required string ShortCode { get; set; }

    public required string OriginalUrl { get; init; }

    public required AppUser CreatedBy { get; init; }
}
