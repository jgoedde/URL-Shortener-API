namespace UrlShortener.Application.Urls;

using Entities;

public interface IUrlsRepository
{
    public Task<Url> CreateUrl(string originalUrl, CancellationToken cancellationToken);

    public Task<Url> GetByShortCode(string shortCode, CancellationToken cancellationToken);
}
