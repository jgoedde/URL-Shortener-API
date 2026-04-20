namespace UrlShortener.Application.Urls;

using SimpleBase;

public class UrlEncoder
{
    public string GetShortCode(int urlId)
    {
        return Base62.Default.Encode(BitConverter.GetBytes(urlId));
    }
}
