using HouseBroker.Application.Repositories;
using HouseBroker.Application.Specifications.Abstracts;
using HouseBroker.Domain;
using HouseBroker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Repositories;

public class PropertyListingReadRepository: IPropertyListingReadRepository
{
    private readonly HouseBrokerDbContext _houseBrokerDbContext;

    public PropertyListingReadRepository(
        HouseBrokerDbContext houseBrokerDbContext
    )
    {
        _houseBrokerDbContext = houseBrokerDbContext;
    }
    
    public async Task<IEnumerable<PropertyListing>> GetAllAsync(IEnumerable<ISpecification<PropertyListing>> specs, CancellationToken cancellationToken)
    {
        var query = _houseBrokerDbContext.PropertyListings.AsQueryable();

        foreach (var spec in specs)
        {
            query = query.Where(spec.Criteria);
        }
        
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PropertyListing>> GetAllAsync(ISpecification<PropertyListing> spec, CancellationToken cancellationToken)
    {
        var query = _houseBrokerDbContext.PropertyListings.AsQueryable();

        query = query.Where(spec.Criteria);
        
        return await query.ToListAsync(cancellationToken);
    }
}