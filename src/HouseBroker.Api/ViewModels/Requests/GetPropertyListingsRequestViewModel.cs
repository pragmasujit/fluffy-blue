using HouseBroker.Domain.Enums;

namespace HouseBroker.Api.ViewModels.Requests;

public record GetPropertyListingsRequestViewModel(
    Guid? Guid,
    PropertyType? PropertyType,
    int? PriceFrom,
    int? PriceTo,
    string? Country,
    string? City,
    string? Street
);
