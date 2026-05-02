namespace UrlShortener.Application.Common.Caching;

public interface ICachedQuery
{
    public string Key { get; }

    public TimeSpan? Expiration { get; }
}

public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery;
