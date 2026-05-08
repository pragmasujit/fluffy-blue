using HouseBroker.Domain.Contexts.HouseBroker.Commission;
using HouseBroker.Domain.Exceptions;

namespace HouseBroker.Domain.Tests
{
    [TestFixture]
    public class CommissionTests
    {
        [Test]
        public void Create_Should_Create_Valid_Commission()
        {
            // Act
            var commission = Commission.Create(0, 5_000_000, 0.02m);

            // Assert
            Assert.AreEqual(0, commission.MinPrice);
            Assert.AreEqual(5_000_000, commission.MaxPrice);
            Assert.AreEqual(0.02m, commission.Percentage);
        }

        [Test]
        public void Create_Should_Allow_Open_Ended_MaxPrice()
        {
            // Act
            var commission = Commission.Create(10_000_000, null, 0.015m);

            // Assert
            Assert.AreEqual(10_000_000, commission.MinPrice);
            Assert.IsNull(commission.MaxPrice);
            Assert.AreEqual(0.015m, commission.Percentage);
        }

        [Test]
        public void Create_Should_Throw_When_MinPrice_Is_Negative()
        {
            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                Commission.Create(-1, 5_000_000, 0.02m));

            Assert.AreEqual("MinPrice cannot be negative", ex!.Message);
        }

        [Test]
        public void Create_Should_Throw_When_Percentage_Is_Invalid()
        {
            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                Commission.Create(0, 5_000_000, 0));

            Assert.AreEqual("Percentage must be greater than 0", ex!.Message);
        }

        [Test]
        public void Create_Should_Throw_When_MaxPrice_Less_Than_MinPrice()
        {
            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                Commission.Create(5_000_000, 1_000_000, 0.02m));

            Assert.AreEqual("MaxPrice cannot be less than MinPrice", ex!.Message);
        }
    }
}