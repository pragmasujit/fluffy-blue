using HouseBroker.Domain.Abstracts;
using HouseBroker.Domain.Exceptions;
using HouseBroker.Domain.DomainValidators;

namespace HouseBroker.Domain.ValueObjects;

public sealed class PropertyListingAddress: ValueObject
{
    public string Street { get; protected set; }
    public string City { get; protected set; }
    public string Country { get; protected set; }
    
    public PropertyListingAddress() { }
    
    public static PropertyListingAddress Create(string street, string city, string country)
    {
        var address = new PropertyListingAddress
        {
            Street = street,
            City = city,
            Country = country
        };

        var validator = new PropertyListingAddressValidator();
        var result = validator.Validate(address);

        if (!result.IsValid)
        {
            var failure = result.Errors[0];
            throw new DomainValidationException(failure.PropertyName, failure.ErrorMessage);
        }

        return address;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return Country;
    }
}

