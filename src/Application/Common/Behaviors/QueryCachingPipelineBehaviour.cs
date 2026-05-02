namespace UrlShortener.Application.Common.Behaviors;

using Caching;
using MediatR;

internal sealed class QueryCachingPipelineBehaviour<TRequest, TResponse>(ICacheService cacheService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        return await cacheService.GetOrCreateAsync(
            request.Key,
            _ => next(cancellationToken),
            request.Expiration,
            cancellationToken
        );
    }
}
