using HouseBroker.Domain;

namespace HouseBroker.Application.Repositories
{
    public interface IPropertyListingWriteRepository
    {
        Task<PropertyListing?> GetByIdAsync(Guid id, string userId, CancellationToken cancellationToken);
        void Add(PropertyListing listing);
        void Update(PropertyListing listing);
        void Remove(PropertyListing listing);
    }
}