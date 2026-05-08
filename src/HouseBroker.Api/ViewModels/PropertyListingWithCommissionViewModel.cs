using HouseBroker.Domain.Misc.Isos;

namespace HouseBroker.Api.ViewModels;

public sealed record PropertyListingWithCommissionViewModel(
    Guid Guid,
    string Name,
    decimal Price,
    string CurrencyCode,
    decimal Commission,
    DateTime CreatedAt,
    string CreatedBy,
    string PropertyTypeName
)
{
    public string CurrencySymbol { get; init; } =
        IsoCurrencies.All.FirstOrDefault(x => x.Code == CurrencyCode).Symbol ?? string.Empty;
}