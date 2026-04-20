namespace UrlShortener.Application.Urls.Queries.GetOriginalUrl;

using System.ComponentModel.DataAnnotations;
using MediatR;

public class GetOriginalUrlQuery : IRequest<string>
{
    [Required]
    public string ShortCode { get; init; }
}
