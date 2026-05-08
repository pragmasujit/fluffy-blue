using HouseBroker.Domain.Enums;

namespace HouseBroker.Api.ViewModels.Requests;

public record UpdatePropertyListingRequestViewModel(
    string Name,
    string CurrencyCode,
    decimal Price,
    PropertyType PropertyType,
    IEnumerable<string> ImageUrls,
    string Street,
    string City,
    string Country
);