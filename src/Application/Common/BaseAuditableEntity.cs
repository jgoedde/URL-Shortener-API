namespace UrlShortener.Application.Common;

public abstract class BaseAuditableEntity
{
    public int Id { get; set; }

    public DateTimeOffset Created { get; set; }

    public DateTimeOffset LastModified { get; set; }
}
