using HouseBroker.Application.Specifications.Abstracts;
using HouseBroker.Domain;

namespace HouseBroker.Application.Repositories;

public interface IPropertyListingReadRepository
{
    Task<IEnumerable<PropertyListing>> GetAllAsync(IEnumerable<ISpecification<PropertyListing>> specs, CancellationToken cancellationToken);
}