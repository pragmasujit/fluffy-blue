using Api.Shared.Exceptions;
using HouseBroker.Application.Commands;
using HouseBroker.Application.Repositories;
using HouseBroker.Domain.Enums;
using HouseBroker.Domain.Misc.Isos;
using HouseBroker.Shared.Subject;
using Moq;
using Microsoft.Extensions.Logging;
using HouseBroker.Application.UnitOfWork;

namespace HouseBroker.Application.Tests.Commands
{
    [TestFixture]
    public class CreatePropertyListingHandlerTests
    {
        private Mock<IPropertyListingWriteRepository> _writeRepositoryMock = null!;
        private Mock<IHouseBrokerUnitOfWork> _unitOfWorkMock = null!;
        private Mock<ILogger<CreatePropertyListing.Handler>> _loggerMock = null!;
        private Subject _subject = null!;
        private CreatePropertyListing.Handler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _writeRepositoryMock = new Mock<IPropertyListingWriteRepository>();
            _unitOfWorkMock = new Mock<IHouseBrokerUnitOfWork>();
            _loggerMock = new Mock<ILogger<CreatePropertyListing.Handler>>();

            _subject = new Subject("user-123");

            _handler = new CreatePropertyListing.Handler(
                _writeRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _subject,
                _loggerMock.Object
            );
        }

        [Test]
        public void Handle_WithInvalidData_ShouldThrowAppValidationException()
        {
            // Arrange
            var command = new CreatePropertyListing.Command(
                Name: "asdfasdf",
                CurrencyCode: IsoCurrencies.UnitedStatesDollar.Code,
                Price: -10,
                PropertyType: PropertyType.House,
                ImageUrls: new List<string>(),
                Country: "Country",
                Street: "Street",
                City: "City"
            );

            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var ex = Assert.ThrowsAsync<AppValidationException>(() =>
                _handler.Handle(command, cancellationToken));

            Assert.That(ex!.Identifier, Is.Not.Null.And.Not.Empty);
            Assert.That(ex.Message, Is.Not.Empty);

            _writeRepositoryMock.Verify(
                r => r.Add(It.IsAny<HouseBroker.Domain.PropertyListing>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);

            _writeRepositoryMock.VerifyNoOtherCalls();
        }
    }
}