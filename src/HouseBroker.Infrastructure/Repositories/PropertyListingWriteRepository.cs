using HouseBroker.Application.Repositories;
using HouseBroker.Domain;
using HouseBroker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Repositories;

public class PropertyListingWriteRepository: IPropertyListingWriteRepository
{
    private readonly HouseBrokerDbContext _houseBrokerDbContext;

    public PropertyListingWriteRepository(HouseBrokerDbContext houseBrokerDbContext)
    {
        _houseBrokerDbContext = houseBrokerDbContext;
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _houseBrokerDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<PropertyListing?> GetByIdAsync(
        Guid id,
        string userId,
        CancellationToken cancellationToken)
    {
        return await _houseBrokerDbContext.PropertyListings
            .FirstOrDefaultAsync(
                x => x.Guid == id && x.CreatedBy == userId,
                cancellationToken);
    }

    public void Add(PropertyListing listing)
    {
        _houseBrokerDbContext.Add(listing);
    }

    public void Update(PropertyListing listing)
    {
        _houseBrokerDbContext.Update(listing);
    }

    public void Remove(PropertyListing propertyListing)
    {
        _houseBrokerDbContext.Remove(propertyListing);
    }
}