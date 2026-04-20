namespace UrlShortener.Application.Urls.Commands.ShortenUrl;

using Entities;
using MediatR;

public record ShortenUrlCommand(string LongUrl) : IRequest<Url>;
