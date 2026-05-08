using FluentValidation;
using HouseBroker.Domain.ValueObjects;
using HouseBroker.Shared.Validation;

namespace HouseBroker.Domain.DomainValidators;

public class PropertyListingAddressValidator : DomainValidator<PropertyListingAddress>
{
    public PropertyListingAddressValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Street is required.");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required.");

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required.");
    }
}