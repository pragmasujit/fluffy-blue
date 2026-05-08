using HouseBroker.Domain.Abstracts;
using HouseBroker.Domain.Enums;
using HouseBroker.Domain.ValueObjects;
using HouseBroker.Shared.Validation;

namespace HouseBroker.Domain;

public class PropertyListing : AuditableEntity
{
    public string Name { get; protected set; }
    public string? Description { get; protected set; }
    public PropertyListingAddress PropertyListingAddress { get; protected set; }
    public decimal Price { get; protected set; }
    public string CurrencyCode { get; protected set; }
    public PropertyType PropertyType { get; protected set; }
    public IEnumerable<string> ImageUrls { get; protected set; } = new List<string>();

    public PropertyListing() { }

    public static PropertyListing Create(
        string name,
        string currencyCode,
        decimal price,
        PropertyType propertyType,
        IEnumerable<string> imageUrls,
        PropertyListingAddress propertyListingAddress,
        string createdBy
    )
    {
        imageUrls ??= new List<string>();

        var propertyListing = new PropertyListing
        {
            Guid = Guid.NewGuid(),
            CreatedBy = createdBy,
            Name = name,
            CurrencyCode = currencyCode,
            Price = price,
            PropertyType = propertyType,
            ImageUrls = imageUrls,
            PropertyListingAddress = propertyListingAddress,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var validator = new PropertyListingValidator();
        
        validator.ValidateAndThrow(propertyListing);
        
        return propertyListing;
    }

    public PropertyListing Update(
        string name,
        string currencyCode,
        decimal price,
        PropertyType propertyType,
        IEnumerable<string> imageUrls,
        string updatedBy,
        PropertyListingAddress propertyListingAddress
    )
    {
        imageUrls ??= new List<string>();

        // DDD does not allow aggregate root to be in an inconsistent state, validating the request first
        var temp = new PropertyListing
        {
            Id = this.Id,
            Guid = this.Guid,
            CreatedAt = this.CreatedAt,
            CreatedBy = this.CreatedBy,

            Name = name,
            CurrencyCode = currencyCode,
            Price = price,
            PropertyType = propertyType,
            ImageUrls = imageUrls.ToList(),
            PropertyListingAddress = propertyListingAddress,

            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = updatedBy
        };

        var validator = new PropertyListingValidator();
        validator.ValidateAndThrow(temp);

        Name = name;
        CurrencyCode = currencyCode;
        Price = price;
        PropertyType = propertyType;
        ImageUrls = imageUrls.ToList();
        PropertyListingAddress = propertyListingAddress;

        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        return this;
    }
}
