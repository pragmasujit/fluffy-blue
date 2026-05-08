using System.Linq.Expressions;
using HouseBroker.Application.Specifications.Abstracts;
using HouseBroker.Domain;
using HouseBroker.Domain.Enums;

namespace HouseBroker.Application.Specifications;

public class ShouldBeCreatedBySpecification: ISpecification<PropertyListing>
{
    public ShouldBeCreatedBySpecification(string userId)
    {
        Criteria = x => x.CreatedBy == userId;
    }
    public Expression<Func<PropertyListing, bool>> Criteria { get; }

    public static ShouldBeCreatedBySpecification Create(string userId) =>
        new(userId);
}