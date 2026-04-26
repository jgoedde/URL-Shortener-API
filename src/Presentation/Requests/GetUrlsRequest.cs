namespace UrlShortener.Presentation.Requests;

public record GetUrlsRequest(int PageSize = 10, int PageNumber = 1);
