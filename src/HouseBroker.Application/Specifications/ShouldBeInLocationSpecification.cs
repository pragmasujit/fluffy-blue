using System.Linq.Expressions;
using HouseBroker.Application.Specifications.Abstracts;
using HouseBroker.Domain;
using HouseBroker.Domain.ValueObjects;

namespace HouseBroker.Application.Specifications;

public class ShouldBeInLocationSpecification: ISpecification<PropertyListing>
{
    public ShouldBeInLocationSpecification(PropertyListingAddress address)
    {
        Criteria = x => x.PropertyListingAddress == address;
    }
    
    public Expression<Func<PropertyListing, bool>> Criteria { get; }

    public static ShouldBeInLocationSpecification Create(PropertyListingAddress address) =>
        new(address);
}