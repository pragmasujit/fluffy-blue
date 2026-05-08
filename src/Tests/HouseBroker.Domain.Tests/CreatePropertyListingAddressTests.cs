using HouseBroker.Domain.Exceptions;
using HouseBroker.Domain.ValueObjects;

namespace HouseBroker.Domain.Tests;

public class CreatePropertyListingAddressTests
{
    private string? _street;
    private string? _city;
    private string? _country;

    private void Initialize()
    {
        _ = PropertyListingAddress.Create(_street!, _city!, _country!);
    }

    [SetUp]
    public void Setup()
    {
        _street = "123 Street";
        _city = "Kathmandu";
        _country = "Nepal";
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void Create_InvalidStreet_ShouldThrowDomainValidationException(string? invalidStreet)
    {
        _street = invalidStreet;

        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.That(ex!.Identifier, Is.EqualTo(nameof(PropertyListingAddress.Street)));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void Create_InvalidCity_ShouldThrowDomainValidationException(string? invalidCity)
    {
        _city = invalidCity;

        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.That(ex!.Identifier, Is.EqualTo(nameof(PropertyListingAddress.City)));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void Create_InvalidCountry_ShouldThrowDomainValidationException(string? invalidCountry)
    {
        _country = invalidCountry;

        var ex = Assert.Throws<DomainValidationException>(() => Initialize());
        Assert.That(ex!.Identifier, Is.EqualTo(nameof(PropertyListingAddress.Country)));
    }

    [Test]
    public void Create_ValidAddress_ShouldSucceed()
    {
        Assert.DoesNotThrow(() => Initialize());
    }
}