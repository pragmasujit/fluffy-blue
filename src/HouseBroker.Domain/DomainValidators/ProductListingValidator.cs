using FluentValidation;
using HouseBroker.Domain;
using HouseBroker.Domain.Misc.Isos;

namespace HouseBroker.Shared.Validation
{
    public class PropertyListingValidator : DomainValidator<PropertyListing>
    {
        public PropertyListingValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Title is required.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.CurrencyCode)
                .NotEmpty().WithMessage("Currency Code is required.")
                .Must(code => IsoCurrencies.All.Any(c => c.Code == code))
                .WithMessage("Currency Code is not valid.");

            When(x => x.ImageUrls != null, () =>
            {
                RuleFor(x => x.ImageUrls!)
                    .Must(urls => urls.Any())
                    .WithMessage("At least one image is required.");

                RuleForEach(x => x.ImageUrls!)
                    .NotEmpty().WithMessage("One or more image URLs are empty.")
                    .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    .WithMessage("One or more image URLs are not valid absolute URIs.");
            });
        }
    }
}