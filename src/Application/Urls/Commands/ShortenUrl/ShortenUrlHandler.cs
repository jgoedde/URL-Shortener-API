namespace UrlShortener.Application.Urls.Commands.ShortenUrl;

using MediatR;

public class ShortenUrlHandler(IUrlsRepository urlsRepository)
    : IRequestHandler<ShortenUrlCommand, string>
{
    public async Task<string> Handle(ShortenUrlCommand request, CancellationToken cancellationToken)
    {
        var url = await urlsRepository.CreateUrl(request.Url, cancellationToken);
        return "hi";
    }
}
