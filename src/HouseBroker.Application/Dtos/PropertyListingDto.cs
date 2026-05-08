using HouseBroker.Domain.Enums;

namespace HouseBroker.Application.Dtos;

public record PropertyListingDto(
    Guid Guid,
    string Name,
    decimal Price,
    string CurrencyCode,
    DateTime CreatedAt,
    string CreatedBy,
    PropertyType PropertyType,
    IEnumerable<string> ImageUrls,
    PropertyListingAddressDto PropertyListingAddress
);

public record PropertyListingAddressDto(
    string Street,
    string City,
    string Country
);

