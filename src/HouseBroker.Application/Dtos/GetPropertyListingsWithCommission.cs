using HouseBroker.Domain.Enums;

namespace HouseBroker.Application.Dtos;

public record PropertyListingWithCommissionDto(
    Guid Guid,
    string Name,
    decimal Price,
    string CurrencyCode,
    DateTime CreatedAt,
    string CreatedBy,
    decimal Commission,
    PropertyType PropertyType,
    IEnumerable<string> ImageUrls,
    PropertyListingAddressDto PropertyListingAddress
);


