namespace UrlShortener.Application.Urls;

using Entities;

public interface IUrlsRepository
{
    public Task<Url> CreateUrl(string originalUrl);
}
