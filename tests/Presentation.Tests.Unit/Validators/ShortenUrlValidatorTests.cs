namespace UrlShortener.Presentation.Tests.Unit.Validators;

using FluentValidation.TestHelper;
using Presentation.Validators;
using Requests;
using Xunit;

public class ShortenUrlValidatorTests
{
    private static readonly ShortenUrlValidator Validator = new();

    [Theory]
    [InlineData("https://example.com")]
    [InlineData("https://example.com/path")]
    [InlineData("https://example.com/path?q=1&r=2")]
    [InlineData("https://example.com/path#fragment")]
    [InlineData("https://sub.example.co.uk/deep/path")]
    public void ValidUrls_ShouldPass(string url)
    {
        var result = Validator.TestValidate(new ShortenUrlRequest { Url = url });
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("http://example.com")] // not https
    [InlineData("ftp://example.com")] // wrong scheme
    [InlineData("https://user@example.com")] // has user info
    [InlineData("https://u:p@example.com")] // has user:password
    [InlineData("not-a-url")] // not a URI at all
    [InlineData("")] // empty
    [InlineData(null)] // null
    public void InvalidUrls_ShouldFail(string? url)
    {
        var result = Validator.TestValidate(new ShortenUrlRequest { Url = url! });
        result.ShouldHaveValidationErrorFor(x => x.Url);
    }
}
