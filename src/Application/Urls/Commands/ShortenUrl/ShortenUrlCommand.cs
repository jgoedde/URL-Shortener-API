namespace UrlShortener.Application.Urls.Commands.ShortenUrl;

using MediatR;

public record ShortenUrlCommand(string Url) : IRequest<string>;
