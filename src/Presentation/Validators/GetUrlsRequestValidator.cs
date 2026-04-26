namespace UrlShortener.Presentation.Validators;

using FluentValidation;
using Requests;

public class GetUrlsRequestValidator : AbstractValidator<GetUrlsRequest>
{
    public GetUrlsRequestValidator()
    {
        _ = this.RuleFor(it => it.PageNumber).GreaterThanOrEqualTo(1);
        _ = this.RuleFor(it => it.PageSize).GreaterThanOrEqualTo(1).LessThanOrEqualTo(25);
    }
}
