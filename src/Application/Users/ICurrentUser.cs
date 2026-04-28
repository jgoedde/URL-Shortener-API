namespace UrlShortener.Application.Users;

public interface ICurrentUser
{
    public string Id { get; }

    public bool IsAuthenticated { get; }
}
