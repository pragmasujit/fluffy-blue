using System.Linq.Expressions;
using HouseBroker.Application.Specifications.Abstracts;
using HouseBroker.Domain;
using HouseBroker.Domain.Enums;

namespace HouseBroker.Application.Specifications;

public class ShouldBePropertyTypeSpecification: ISpecification<PropertyListing>
{
    public ShouldBePropertyTypeSpecification(PropertyType propertyType)
    {
        Criteria = x => x.PropertyType == propertyType;
    }
    public Expression<Func<PropertyListing, bool>> Criteria { get; }

    public static ShouldBePropertyTypeSpecification Create(PropertyType propertyType) =>
        new(propertyType);
}