using System.Linq.Expressions;
using HouseBroker.Application.Specifications.Abstracts;
using HouseBroker.Domain;

namespace HouseBroker.Application.Specifications;

public class ShouldHaveGuidSpecification: ISpecification<PropertyListing>
{
    public ShouldHaveGuidSpecification(Guid guid)
    {
        Criteria = x => x.Guid == guid;
    }
    
    public Expression<Func<PropertyListing, bool>> Criteria { get; }

    public static ShouldHaveGuidSpecification Create(Guid guid) =>
        new(guid);
}