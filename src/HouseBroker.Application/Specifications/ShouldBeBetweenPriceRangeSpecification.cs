using System.Linq.Expressions;
using HouseBroker.Application.Specifications.Abstracts;
using HouseBroker.Domain;

namespace HouseBroker.Application.Specifications
{
    public class ShouldBeBetweenPriceRangeSpecification : ISpecification<PropertyListing>
    {
        public Expression<Func<PropertyListing, bool>> Criteria { get; }

        public ShouldBeBetweenPriceRangeSpecification(decimal minPrice, decimal maxPrice)
        {
            if (minPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(minPrice), "minPrice cannot be negative.");
            if (maxPrice < minPrice)
                throw new ArgumentOutOfRangeException(nameof(maxPrice), "maxPrice must be >= minPrice.");

            Criteria = listing =>
                listing.Price >= minPrice &&
                listing.Price <= maxPrice;
        }

        public static ShouldBeBetweenPriceRangeSpecification Create(decimal minPrice, decimal maxPrice) =>
            new(minPrice, maxPrice);
    }
}