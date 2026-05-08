using HouseBroker.Application.Queries;
using HouseBroker.Application.Repositories;
using HouseBroker.Application.Specifications.Abstracts;
using HouseBroker.Domain;
using HouseBroker.Domain.Enums;
using HouseBroker.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

namespace HouseBroker.Application.Tests.Queries;

[TestFixture]
public class GetPropertyListingsHandlerTests
{
    private Mock<IPropertyListingReadRepository> _repositoryMock = null!;
    private Mock<ILogger<GetPublicPropertyListings.Handler>> _loggerMock = null!;
    private GetPublicPropertyListings.Handler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IPropertyListingReadRepository>();
        _loggerMock = new Mock<ILogger<GetPublicPropertyListings.Handler>>();
        _handler = new GetPublicPropertyListings.Handler(_repositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_WithNullGuid_ShouldCallRepositoryWithEmptySpecs()
    {
        // Arrange
        var query = new GetPublicPropertyListings.Query(
            Guid: null,
            PropertyType: null,
            PriceFrom: null,
            PriceTo: null,
            Country: null,
            City: null,
            Street: null
        );
        var cancellationToken = CancellationToken.None;

        _repositoryMock
            .Setup(repo => repo.GetAllAsync(
                It.Is<IEnumerable<ISpecification<PropertyListing>>>(specs => !specs.Any()),
                cancellationToken
            ))
            .ReturnsAsync(new List<PropertyListing>());

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        _repositoryMock.Verify(repo =>
            repo.GetAllAsync(It.IsAny<IEnumerable<ISpecification<PropertyListing>>>(), cancellationToken),
            Times.Once);
        Assert.IsEmpty(result);
    }

    [Test]
    public async Task Handle_WithGuid_ShouldCallRepositoryAndReturnFilteredResult()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var query = new GetPublicPropertyListings.Query(
            Guid: guid,
            PropertyType: null,
            PriceFrom: null,
            PriceTo: null,
            Country: null,
            City: null,
            Street: null
        );
        var cancellationToken = CancellationToken.None;

        var propertyListing = PropertyListing.Create(
            name: "Test Property",
            currencyCode: "USD",
            price: 150000,
            propertyType: PropertyType.House,
            createdBy: "123132",
            imageUrls: new List<string> { "https://cdn.test.com/photo.jpg" },
            propertyListingAddress: PropertyListingAddress.Create("Road", "City", "Country")
        );

        _repositoryMock
            .Setup(repo => repo.GetAllAsync(
                It.IsAny<IEnumerable<ISpecification<PropertyListing>>>(),
                cancellationToken
            ))
            .ReturnsAsync(new List<PropertyListing> { propertyListing });

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        _repositoryMock.Verify(repo =>
            repo.GetAllAsync(It.IsAny<IEnumerable<ISpecification<PropertyListing>>>(), cancellationToken),
            Times.Once);

        var dto = result.First();
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("Test Property", dto.Name);
        Assert.AreEqual("USD", dto.CurrencyCode);
        Assert.AreEqual(150000, dto.Price);
        Assert.AreEqual(PropertyType.House, dto.PropertyType);
        Assert.Contains("https://cdn.test.com/photo.jpg", dto.ImageUrls.ToList());
        Assert.AreEqual("Road", dto.PropertyListingAddress.Street);
        Assert.AreEqual("City", dto.PropertyListingAddress.City);
        Assert.AreEqual("Country", dto.PropertyListingAddress.Country);
    }

    [Test]
    public async Task Handle_WithGuidButNoMatchingListing_ShouldReturnEmpty()
    {
        // Arrange
        var query = new GetPublicPropertyListings.Query(
            Guid: Guid.NewGuid(),
            PropertyType: null,
            PriceFrom: null,
            PriceTo: null,
            Country: null,
            City: null,
            Street: null
        );
        var cancellationToken = CancellationToken.None;

        _repositoryMock
            .Setup(repo => repo.GetAllAsync(
                It.IsAny<IEnumerable<ISpecification<PropertyListing>>>(),
                cancellationToken
            ))
            .ReturnsAsync(new List<PropertyListing>());

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        _repositoryMock.Verify(repo =>
            repo.GetAllAsync(It.IsAny<IEnumerable<ISpecification<PropertyListing>>>(), cancellationToken),
            Times.Once);
        Assert.IsEmpty(result);
    }
}
