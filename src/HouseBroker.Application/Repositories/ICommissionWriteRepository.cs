using HouseBroker.Domain;
using HouseBroker.Domain.Contexts.HouseBroker.Commission;

namespace HouseBroker.Application.Repositories
{
    public interface ICommissionWriteRepository
    {
        Task<IEnumerable<Commission>> GetAllAsync(CancellationToken cancellationToken);
    }
}