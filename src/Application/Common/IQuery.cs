namespace UrlShortener.Application.Common;

using MediatR;

public interface IQuery<TResponse> : IRequest<TResponse>;
