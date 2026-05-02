namespace UrlShortener.Presentation.Validators;

using FluentValidation;
using Requests;

public class ShortenUrlValidator : AbstractValidator<ShortenUrlRequest>
{
    public ShortenUrlValidator()
    {
        _ = this.RuleFor(it => it.Url)
            .NotEmpty()
            .Must(BeValidUrl)
            .When(x => !string.IsNullOrEmpty(x.Url), ApplyConditionTo.CurrentValidator);
    }

    private static bool BeValidUrl(string urlRaw)
    {
        if (!Uri.TryCreate(urlRaw, UriKind.Absolute, out var uri))
        {
            return false;
        }

        if (uri.Scheme != Uri.UriSchemeHttps)
        {
            return false;
        }

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (!string.IsNullOrEmpty(uri.UserInfo))
        {
            return false;
        }

        return true;
    }
}
