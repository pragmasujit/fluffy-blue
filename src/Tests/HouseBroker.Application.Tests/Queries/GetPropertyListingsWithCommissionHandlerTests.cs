using HouseBroker.Application.Queries;
using HouseBroker.Application.Repositories;
using HouseBroker.Application.Services;
using HouseBroker.Application.Specifications.Abstracts;
using HouseBroker.Domain;
using HouseBroker.Domain.Contexts.HouseBroker.Commission;
using HouseBroker.Domain.Enums;
using HouseBroker.Domain.ValueObjects;
using HouseBroker.Shared.Subject;
using Microsoft.Extensions.Logging;
using Moq;

namespace HouseBroker.Application.Tests.Queries
{
    [TestFixture]
    public class GetPropertyListingsWithCommissionHandlerTests
    {
        private Mock<IPropertyListingReadRepository> _readRepoMock = null!;
        private Mock<ICommissionWriteRepository> _commissionRepoMock = null!;
        private Mock<ICommissionEngine> _engineMock = null!;
        private Mock<ILogger<GetPropertyListingsWithCommission.Handler>> _loggerMock = null!;
        private Subject _subject = null!;
        private GetPropertyListingsWithCommission.Handler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _readRepoMock = new Mock<IPropertyListingReadRepository>();
            _commissionRepoMock = new Mock<ICommissionWriteRepository>();
            _engineMock = new Mock<ICommissionEngine>();
            _loggerMock = new Mock<ILogger<GetPropertyListingsWithCommission.Handler>>();

            _subject = new Subject("user-123");

            _handler = new GetPropertyListingsWithCommission.Handler(
                _readRepoMock.Object,
                _engineMock.Object,
                _commissionRepoMock.Object,
                _subject,
                _loggerMock.Object
            );
        }

        private PropertyListing CreateListing(decimal price)
        {
            var listing = PropertyListing.Create(
                name: "Test Property",
                currencyCode: "USD",
                price: price,
                propertyType: PropertyType.Apartment,
                imageUrls: new List<string> { "https://image.com/1.jpg" },
                propertyListingAddress: PropertyListingAddress.Create("street", "city", "country"),
                createdBy: "user-123"
            );

            return listing;
        }

        [Test]
        public async Task Handle_Should_Return_Listings_With_Commission()
        {
            // Arrange
            var listing = CreateListing(1_000_000m);

            _readRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<IEnumerable<ISpecification<PropertyListing>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PropertyListing> { listing });

            var commissionRules = new List<Commission>
            {
                Commission.Create(0, 5_000_000, 0.02m)
            };

            _commissionRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(commissionRules);

            _engineMock
                .Setup(e => e.CalculateCommissionAsync(1_000_000m, It.IsAny<CancellationToken>()))
                .ReturnsAsync(20_000m);

            // Act
            var result = await _handler.Handle(
                new GetPropertyListingsWithCommission.Query(
                    null, null, null, null, null, null, null),
                CancellationToken.None
            );

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(20_000m, result.First().Commission);
        }

        [Test]
        public async Task Handle_Should_Return_Empty_When_No_Listings()
        {
            // Arrange
            _readRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<IEnumerable<ISpecification<PropertyListing>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PropertyListing>());

            _commissionRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Commission>());

            _engineMock
                .Setup(e => e.CalculateCommissionAsync(It.IsAny<decimal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(
                new GetPropertyListingsWithCommission.Query(null, null, null, null, null, null, null),
                CancellationToken.None
            );

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }
    }
}