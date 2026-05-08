using HouseBroker.Application.Dtos;
using HouseBroker.Domain;

namespace HouseBroker.Application.Extensions;

public static class DtoMappingExtensions
{
    public static PropertyListingDto ToDto(
        this PropertyListing propertyListing
    )
    {
        return new PropertyListingDto(
            Guid: propertyListing.Guid,
            Name: propertyListing.Name,
            Price: propertyListing.Price,
            CurrencyCode: propertyListing.CurrencyCode,
            CreatedAt: propertyListing.CreatedAt,
            CreatedBy: propertyListing.CreatedBy,
            PropertyType: propertyListing.PropertyType,
            ImageUrls: propertyListing.ImageUrls,
            PropertyListingAddress: new PropertyListingAddressDto(
                Street: propertyListing.PropertyListingAddress.Street,
                City: propertyListing.PropertyListingAddress.City,
                Country: propertyListing.PropertyListingAddress.Country
            )
        );
    }
    
    public static PropertyListingWithCommissionDto ToDto(
        this PropertyListing propertyListing,
        decimal commission
    )
    {
        return new PropertyListingWithCommissionDto(
            Guid: propertyListing.Guid,
            Name: propertyListing.Name,
            Price: propertyListing.Price,
            CurrencyCode: propertyListing.CurrencyCode,
            Commission: commission,
            CreatedAt: propertyListing.CreatedAt,
            CreatedBy: propertyListing.CreatedBy,
            PropertyType: propertyListing.PropertyType,
            ImageUrls: propertyListing.ImageUrls,
            PropertyListingAddress: new PropertyListingAddressDto(
                Street: propertyListing.PropertyListingAddress.Street,
                City: propertyListing.PropertyListingAddress.City,
                Country: propertyListing.PropertyListingAddress.Country
            )
        );
    }
}