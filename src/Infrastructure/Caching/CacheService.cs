namespace UrlShortener.Infrastructure.Caching;

using System.Text.Json;
using Application.Common.Caching;
using StackExchange.Redis;

internal sealed class CacheService(IConnectionMultiplexer multiplexer) : ICacheService
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(10);

    public async Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default
    )
    {
        var database = multiplexer.GetDatabase();

        var cached = await database.StringGetAsync(key);
        if (cached.HasValue)
        {
            var cacheValue = JsonSerializer.Deserialize<T>(cached.ToString());
            if (cacheValue is not null)
            {
                return cacheValue;
            }

            await database.KeyDeleteAsync(key);
        }

        var value = await factory(cancellationToken);

        await database.StringSetAsync(
            key,
            JsonSerializer.Serialize(value),
            expiration ?? DefaultExpiration
        );

        return value;
    }
}
