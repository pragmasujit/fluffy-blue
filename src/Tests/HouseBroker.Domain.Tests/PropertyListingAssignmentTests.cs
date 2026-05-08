using HouseBroker.Domain.Enums;
using HouseBroker.Domain.Misc.Isos;
using HouseBroker.Domain.ValueObjects;

namespace HouseBroker.Domain.Tests
{
    [TestFixture]
    public class PropertyListingAssignmentTests
    {
        private PropertyListingAddress _address = PropertyListingAddress.Create("123 Street", "City", "Country");
        private List<string> _imageUrls = new() { "https://image1.com", "https://image2.com" };

        [Test]
        public void Create_ShouldAssignProperties()
        {
            var listing = PropertyListing.Create(
                name: "Test Property",
                currencyCode: IsoCurrencies.UnitedStatesDollar.Code,
                price: 100000m,
                propertyType: PropertyType.House,
                imageUrls: _imageUrls,
                propertyListingAddress: _address,
                createdBy: "123123"
            );

            Assert.AreEqual("Test Property", listing.Name);
            Assert.AreEqual("USD", listing.CurrencyCode);
            Assert.AreEqual(100000m, listing.Price);
            Assert.AreEqual(PropertyType.House, listing.PropertyType);
            Assert.AreEqual(_imageUrls, listing.ImageUrls);
            Assert.AreEqual(_address, listing.PropertyListingAddress);
        }

        [Test]
        public void Update_ShouldAssignNewProperties()
        {
            var original = PropertyListing.Create(
                name: "Old Name",
                currencyCode: "USD",
                price: 80000m,
                propertyType: PropertyType.Apartment,
                imageUrls: _imageUrls,
                propertyListingAddress: _address,
                createdBy: "123123"
            );

            var newAddress = PropertyListingAddress.Create("456 New Street", "New City", "New Country");
            var newImageUrls = new List<string> { "https://newimage.com" };

            var updated = original.Update(
                name: "New Name",
                currencyCode: "NPR",
                price: 120000m,
                propertyType: PropertyType.House,
                imageUrls: newImageUrls,
                propertyListingAddress: newAddress,
                updatedBy: "123123"
            );

            Assert.AreEqual("New Name", updated.Name);
            Assert.AreEqual("NPR", updated.CurrencyCode);
            Assert.AreEqual(120000m, updated.Price);
            Assert.AreEqual(PropertyType.House, updated.PropertyType);
            Assert.AreEqual(newImageUrls, updated.ImageUrls);
            Assert.AreEqual(newAddress, updated.PropertyListingAddress);
        }
    }
}
