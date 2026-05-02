namespace UrlShortener.Infrastructure.Caching;

using System.Text.Json;
using Application.Common.Caching;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

internal sealed partial class CacheService(
    IConnectionMultiplexer multiplexer,
    ILogger<CacheService> logger
) : ICacheService
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
                this.LogCacheHit(key);
                return cacheValue;
            }

            await database.KeyDeleteAsync(key);
        }

        this.LogCacheMiss(key);

        var value = await factory(cancellationToken);

        await database.StringSetAsync(
            key,
            JsonSerializer.Serialize(value),
            expiration ?? DefaultExpiration
        );

        return value;
    }

    [LoggerMessage(LogLevel.Debug, "Cache hit for key {Key}")]
    partial void LogCacheHit(string key);

    [LoggerMessage(LogLevel.Debug, "Cache miss for key {Key}")]
    partial void LogCacheMiss(string key);
}
