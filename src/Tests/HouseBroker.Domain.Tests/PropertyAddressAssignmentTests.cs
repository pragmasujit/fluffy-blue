using HouseBroker.Domain.Exceptions;
using HouseBroker.Domain.ValueObjects;

namespace HouseBroker.Domain.Tests
{
    [TestFixture]
    public class PropertyListingAddressTests
    {
        [Test]
        public void Create_WithValidProperties_ShouldAssignProperties()
        {
            var address = PropertyListingAddress.Create("123 Main St", "Kathmandu", "Nepal");

            Assert.AreEqual("123 Main St", address.Street);
            Assert.AreEqual("Kathmandu", address.City);
            Assert.AreEqual("Nepal", address.Country);
        }
    }
}