namespace UrlShortener.Application;

public interface ISequenceService
{
    public Task<int> NextUrlIdAsync(CancellationToken cancellationToken);
}
