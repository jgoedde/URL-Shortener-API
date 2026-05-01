namespace UrlShortener.Application.Urls;

public interface ISequenceService
{
    public Task<int> NextUrlIdAsync(CancellationToken cancellationToken);
}
