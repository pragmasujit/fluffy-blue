using Api.Shared.Exceptions;
using HouseBroker.Application.Commands;
using HouseBroker.Application.Repositories;
using HouseBroker.Application.UnitOfWork;
using HouseBroker.Domain;
using HouseBroker.Domain.Enums;
using HouseBroker.Domain.Misc.Isos;
using HouseBroker.Domain.ValueObjects;
using HouseBroker.Shared.Subject;
using Microsoft.Extensions.Logging;
using Moq;

namespace HouseBroker.Application.Tests.Commands
{
    [TestFixture]
    public class UpdatePropertyListingHandlerTests
    {
        private Mock<IPropertyListingWriteRepository> _writeRepoMock = null!;
        private Mock<IHouseBrokerUnitOfWork> _unitOfWorkMock = null!;
        private Mock<ILogger<UpdatePropertyListing.Handler>> _loggerMock = null!;
        private Subject _subject = null!;
        private UpdatePropertyListing.Handler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _writeRepoMock = new Mock<IPropertyListingWriteRepository>();
            _unitOfWorkMock = new Mock<IHouseBrokerUnitOfWork>();
            _loggerMock = new Mock<ILogger<UpdatePropertyListing.Handler>>();

            _subject = new Subject("user-123");

            _handler = new UpdatePropertyListing.Handler(
                _writeRepoMock.Object,
                _unitOfWorkMock.Object,
                _subject,
                _loggerMock.Object);
        }

        private UpdatePropertyListing.Command CreateValidCommand(Guid guid)
            => new UpdatePropertyListing.Command(
                Guid: guid,
                Name: "Updated Property",
                CurrencyCode: IsoCurrencies.UnitedStatesDollar.Code,
                Price: 1000m,
                PropertyType: PropertyType.Apartment,
                ImageUrls: new List<string> { "https://image1.jpg", "https://image2.jpg" },
                Country: "USA",
                Street: "123 Main St",
                City: "New York"
            );

        private PropertyListing CreateRealListing(Guid guid, string userId)
        {
            var address = PropertyListingAddress.Create("InitialCountry", "InitialStreet", "InitialCity");

            var listing = PropertyListing.Create(
                name: "Initial Property",
                currencyCode: IsoCurrencies.UnitedStatesDollar.Code,
                price: 500m,
                propertyType: PropertyType.Apartment,
                imageUrls: new List<string> { "https://oldimage.jpg" },
                propertyListingAddress: address,
                createdBy: userId);

            typeof(PropertyListing)
                .GetProperty("Guid")!
                .SetValue(listing, guid);

            return listing;
        }

        [Test]
        public async Task Handle_Should_Run_Successfully_When_Listing_Found()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var command = CreateValidCommand(guid);
            var listing = CreateRealListing(guid, "user-123");

            _writeRepoMock
                .Setup(r => r.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(listing);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(guid, result.Guid);

            _writeRepoMock.Verify(r =>
                r.GetByIdAsync(
                    guid,
                    "user-123",
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _writeRepoMock.Verify(w => w.Update(It.IsAny<PropertyListing>()), Times.Once);

            _unitOfWorkMock.Verify(u =>
                u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public void Handle_Should_Throw_When_Listing_Not_Found()
        {
            // Arrange
            var command = CreateValidCommand(Guid.NewGuid());

            _writeRepoMock
                .Setup(r => r.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((PropertyListing?)null);

            // Act & Assert
            Assert.ThrowsAsync<AppValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            _unitOfWorkMock.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}