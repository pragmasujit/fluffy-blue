using HouseBroker.Domain.Enums;
using HouseBroker.Domain.Exceptions;
using HouseBroker.Domain.Misc.Isos;
using HouseBroker.Domain.ValueObjects;

namespace HouseBroker.Domain.Tests;

public class UpdatePropertyListingAddressTests
{
    private PropertyListing _original;
    private string? _name;
    private string? _currencyCode;
    private decimal _price;
    private PropertyType _propertyType;
    private IEnumerable<string>? _imageUrls;
    private PropertyListingAddress _address;

    private void Initialize()
    {
        _ = _original.Update(
            name: _name!,
            currencyCode: _currencyCode!,
            price: _price,
            propertyType: _propertyType,
            imageUrls: _imageUrls!,
            propertyListingAddress: _address,
            updatedBy: "13123"
        );
    }

    [SetUp]
    public void Setup()
    {
        _original = PropertyListing.Create(
            name: "Initial",
            currencyCode: IsoCurrencies.UnitedStatesDollar.Code,
            price: 50000,
            propertyType: PropertyType.House,
            imageUrls: new List<string> { "https://img.com/initial.jpg" },
            createdBy: "123123",
            propertyListingAddress: PropertyListingAddress.Create(
                street: "Init Street",
                city: "Kathmandu",
                country: "Nepal"
            )
        );

        _name = "Updated Name";
        _currencyCode = IsoCurrencies.UnitedStatesDollar.Code;
        _price = 100000;
        _propertyType = PropertyType.Apartment;
        _imageUrls = new List<string> { "https://img.com/1.jpg" };
        _address = PropertyListingAddress.Create(
            street: "New Street",
            city: "Pokhara",
            country: "Nepal"
        );
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void Update_InvalidName_ShouldThrow(string? invalidName)
    {
        _name = invalidName;
        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.AreEqual(nameof(PropertyListing.Name), ex!.Identifier);
    }

    [TestCase("")]
    [TestCase("U")]
    [TestCase("INVALID")]
    [TestCase(null)]
    public void Update_InvalidCurrencyCode_ShouldThrow(string? invalidCode)
    {
        _currencyCode = invalidCode;
        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.AreEqual(nameof(PropertyListing.CurrencyCode), ex!.Identifier);
    }

    [TestCase(-1)]
    [TestCase(0)]
    public void Update_InvalidPrice_ShouldThrow(decimal invalidPrice)
    {
        _price = invalidPrice;
        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.AreEqual(nameof(PropertyListing.Price), ex!.Identifier);
    }

    [Test]
    public void Update_EmptyImageUrls_ShouldThrow()
    {
        _imageUrls = new List<string>();
        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.AreEqual(nameof(PropertyListing.ImageUrls), ex!.Identifier);
    }

    private static readonly object[] InvalidImageUrlsCases = 
    {
        new object[] { new string[] { "" } },
        new object[] { new string[] { "not-a-url" } }
    };

    [Test, TestCaseSource(nameof(InvalidImageUrlsCases))]
    public void Update_ImageUrlsWithInvalidValue_ShouldThrow(string[] invalidUrls)
    {
        _imageUrls = invalidUrls;
        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.AreEqual(nameof(PropertyListing.ImageUrls), ex!.Identifier);
    }

    private static readonly object[] SomeInvalidUrlsCases = 
    {
        new object[] { new string[] { "https://valid.com/1.jpg", "invalid" } }
    };

    [Test, TestCaseSource(nameof(SomeInvalidUrlsCases))]
    public void Update_MultipleImageUrls_SomeInvalid_ShouldThrow(string[] urls)
    {
        _imageUrls = urls;
        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.AreEqual(nameof(PropertyListing.ImageUrls), ex!.Identifier);
    }

    private static readonly object[] AllValidUrlsCases = 
    {
        new object[] { new string[] { "https://valid.com/1.jpg", "https://valid.com/2.jpg" } }
    };

    [Test, TestCaseSource(nameof(AllValidUrlsCases))]
    public void Update_MultipleImageUrls_AllValid_ShouldSucceed(string[] urls)
    {
        _imageUrls = urls;
        Assert.DoesNotThrow(() => Initialize());
    }

    [Test]
    public void Update_ValidData_ShouldSucceed()
    {
        Assert.DoesNotThrow(() => Initialize());
    }

    [Test]
    public void Update_ShouldPreserveCreatedAt_AndUpdateUpdatedAt()
    {
        var beforeUpdate = _original.UpdatedAt;
        var updated = _original.Update(
            name: _name!,
            currencyCode: _currencyCode!,
            price: _price,
            propertyType: _propertyType,
            imageUrls: _imageUrls!,
            updatedBy: "123123",
            propertyListingAddress: _address
        );

        Assert.AreEqual(_original.CreatedAt, updated.CreatedAt);
        Assert.Greater(updated.UpdatedAt, beforeUpdate);
    }
}
