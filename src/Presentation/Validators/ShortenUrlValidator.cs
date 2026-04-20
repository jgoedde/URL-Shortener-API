namespace UrlShortener.Presentation.Validators;

using FluentValidation;
using Requests;

public class ShortenUrlValidator : AbstractValidator<ShortenUrlRequest>
{
    public ShortenUrlValidator()
    {
        _ = this.RuleFor(it => it.Url)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.Url));
    }
}
