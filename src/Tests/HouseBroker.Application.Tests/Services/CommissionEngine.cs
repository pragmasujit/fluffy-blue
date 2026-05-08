using Moq;
using NUnit.Framework;
using HouseBroker.Application.Services;
using HouseBroker.Application.Providers.CommissionProvider;
using HouseBroker.Domain.Contexts.HouseBroker.Commission;

namespace HouseBroker.Application.Tests.Services
{
    [TestFixture]
    public class CommissionEngineTests
    {
        private Mock<ICommissionProvider> _providerMock = null!;
        private CommissionEngine _engine = null!;

        [SetUp]
        public void Setup()
        {
            _providerMock = new Mock<ICommissionProvider>();
            _engine = new CommissionEngine(_providerMock.Object);
        }

        [Test]
        public async Task CalculateCommissionAsync_ShouldReturnCorrectValue_WhenRuleMatches()
        {
            // Arrange
            var price = 1_000_000m;

            var rules = new List<Commission>
            {
                Commission.Create(0, 5_000_000, 0.02m)
            };

            _providerMock
                .Setup(x => x.GetRulesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(rules);

            // Act
            var result = await _engine.CalculateCommissionAsync(price, CancellationToken.None);

            // Assert
            Assert.AreEqual(20_000m, result);
        }

        [Test]
        public async Task CalculateCommissionAsync_ShouldSelectCorrectRule_WhenMultipleRulesExist()
        {
            // Arrange
            var price = 6_000_000m;

            var rules = new List<Commission>
            {
                Commission.Create(0, 5_000_000, 0.02m),
                Commission.Create(5_000_001, 10_000_000, 0.0175m)
            };

            _providerMock
                .Setup(x => x.GetRulesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(rules);

            // Act
            var result = await _engine.CalculateCommissionAsync(price, CancellationToken.None);

            // Assert
            Assert.AreEqual(105_000m, result);
        }

        [Test]
        public async Task CalculateCommissionAsync_ShouldHandleOpenEndedMaxPrice()
        {
            // Arrange
            var price = 50_000_000m;

            var rules = new List<Commission>
            {
                Commission.Create(10_000_000, null, 0.015m)
            };

            _providerMock
                .Setup(x => x.GetRulesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(rules);

            // Act
            var result = await _engine.CalculateCommissionAsync(price, CancellationToken.None);

            // Assert
            Assert.AreEqual(750_000m, result);
        }

        [Test]
        public void CalculateCommissionAsync_ShouldThrow_WhenNoRuleMatches()
        {
            // Arrange
            var price = 1_000_000m;

            _providerMock
                .Setup(x => x.GetRulesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Commission>());

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() =>
                _engine.CalculateCommissionAsync(price, CancellationToken.None));

            Assert.AreEqual("No commission rule found", ex!.Message);
        }
    }
}