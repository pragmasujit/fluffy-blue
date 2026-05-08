using HouseBroker.Domain.Exceptions;
using HouseBroker.Domain.Enums;
using HouseBroker.Domain.Misc.Isos;
using HouseBroker.Domain.ValueObjects;

namespace HouseBroker.Domain.Tests;

public class CreateProductListingTests
{
    private string? _name;
    private string? _currencyCode;
    private decimal _price;
    private PropertyType _propertyType;
    private IEnumerable<string>? _imageUrls;
    private PropertyListingAddress _address;

    private void Initialize()
    {
        _ = PropertyListing.Create(
            name: _name!,
            currencyCode: _currencyCode!,
            price: _price,
            propertyType: _propertyType,
            imageUrls: _imageUrls,
            createdBy: "123123",
            propertyListingAddress: _address
        );
    }

    [SetUp]
    public void Setup()
    {
        _name = "Default Name";
        _currencyCode = IsoCurrencies.UnitedStatesDollar.Code;
        _price = 100000;
        _propertyType = PropertyType.House;
        _imageUrls = new List<string> { "https://img.com/1.jpg" };
        _address = PropertyListingAddress.Create(
            street: "123 Street",
            city: "Kathmandu",
            country: "Nepal"
        );
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void Create_InvalidName_ShouldThrow(string? invalidName)
    {
        _name = invalidName;
        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.That(ex?.Identifier, Is.EqualTo("Name"));
    }

    [TestCase("")]
    [TestCase("U")]
    [TestCase("INVALID")]
    [TestCase(null)]
    public void Create_InvalidCurrencyCode_ShouldThrow(string? invalidCode)
    {
        _currencyCode = invalidCode;
        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.That(ex?.Identifier, Is.EqualTo("CurrencyCode"));
    }

    [TestCase(-1)]
    [TestCase(0)]
    public void Create_InvalidPrice_ShouldThrow(decimal invalidPrice)
    {
        _price = invalidPrice;
        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.That(ex?.Identifier, Is.EqualTo("Price"));
    }

    [Test]
    public void Create_EmptyImageUrls_ShouldThrow()
    {
        _imageUrls = new List<string>();
        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.That(ex?.Identifier, Is.EqualTo("ImageUrls"));
    }

    [Test]
    public void Create_ImageUrlEmptyString_ShouldThrow()
    {
        _imageUrls = new List<string> { "" };
        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.That(ex?.Identifier, Is.EqualTo("ImageUrls"));
    }

    [Test]
    public void Create_ImageUrlInvalidUri_ShouldThrow()
    {
        _imageUrls = new List<string> { "invalid-url" };
        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.That(ex?.Identifier, Is.EqualTo("ImageUrls"));
    }

    [Test]
    public void Create_MultipleImageUrls_SomeInvalid_ShouldThrow()
    {
        _imageUrls = new List<string>
        {
            "https://valid.com/img.jpg",
            "bad-url"
        };
        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.That(ex?.Identifier, Is.EqualTo("ImageUrls"));
    }

    [Test]
    public void Create_MultipleImageUrls_AllValid_ShouldSucceed()
    {
        _imageUrls = new List<string>
        {
            "https://valid.com/1.jpg",
            "https://valid.com/2.jpg"
        };
        Assert.DoesNotThrow(() => Initialize());
    }

    [Test]
    public void Create_ValidData_ShouldSucceed()
    {
        Assert.DoesNotThrow(() => Initialize());
    }
}
