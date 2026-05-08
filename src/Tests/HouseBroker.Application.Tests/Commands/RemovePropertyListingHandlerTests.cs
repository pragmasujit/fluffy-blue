using Api.Shared.Exceptions;
using HouseBroker.Application.Commands;
using HouseBroker.Application.Repositories;
using HouseBroker.Domain;
using HouseBroker.Domain.Enums;
using HouseBroker.Domain.Misc.Isos;
using HouseBroker.Domain.ValueObjects;
using HouseBroker.Application.UnitOfWork;
using HouseBroker.Shared.Subject;
using Microsoft.Extensions.Logging;
using Moq;

namespace HouseBroker.Application.Tests.Commands
{
    [TestFixture]
    public class RemovePropertyListingHandlerTests
    {
        private Mock<IPropertyListingWriteRepository> _writeRepoMock = null!;
        private Mock<IHouseBrokerUnitOfWork> _unitOfWorkMock = null!;
        private Mock<ILogger<RemovePropertyListing.Handler>> _loggerMock = null!;
        private Subject _subject = null!;
        private RemovePropertyListing.Handler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _writeRepoMock = new Mock<IPropertyListingWriteRepository>();
            _unitOfWorkMock = new Mock<IHouseBrokerUnitOfWork>();
            _loggerMock = new Mock<ILogger<RemovePropertyListing.Handler>>();

            _subject = new Subject("user-123");

            _handler = new RemovePropertyListing.Handler(
                _writeRepoMock.Object,
                _unitOfWorkMock.Object,
                _subject,
                _loggerMock.Object);
        }

        private RemovePropertyListing.Command CreateCommand(Guid guid)
            => new RemovePropertyListing.Command(guid);

        [Test]
        public async Task Handle_Should_Remove_Listing_And_Save()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var userId = "user-123";

            var command = CreateCommand(guid);

            var listing = PropertyListing.Create(
                name: "name",
                currencyCode: IsoCurrencies.UnitedStatesDollar.Code,
                price: 123m,
                propertyType: PropertyType.Apartment,
                imageUrls: new List<string>
                {
                    "https://asfas/image.asdfasdf",
                    "https://asfas/imasdfsdfge.asdfasdf"
                },
                propertyListingAddress: PropertyListingAddress.Create(
                    country: "country",
                    street: "street",
                    city: "city"
                ),
                createdBy: userId
            );

            typeof(PropertyListing)
                .GetProperty("Guid")!
                .SetValue(listing, guid);

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

            _writeRepoMock.Verify(r =>
                r.GetByIdAsync(
                    guid,
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _writeRepoMock.Verify(r =>
                r.Remove(listing),
                Times.Once);

            _unitOfWorkMock.Verify(u =>
                u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            _writeRepoMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Handle_Should_Throw_When_Listing_Not_Found()
        {
            // Arrange
            var command = CreateCommand(Guid.NewGuid());

            _writeRepoMock
                .Setup(r => r.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((PropertyListing?)null);

            // Act & Assert
            Assert.ThrowsAsync<AppValidationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            _unitOfWorkMock.Verify(u =>
                u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}