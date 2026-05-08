using HouseBroker.Application.Repositories;
using HouseBroker.Domain.Contexts.HouseBroker.Commission;
using HouseBroker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Repositories;

public class CommissionWriteRepository: ICommissionWriteRepository
{
    private readonly HouseBrokerDbContext _houseBrokerDbContext;

    public CommissionWriteRepository(
        HouseBrokerDbContext houseBrokerDbContext
    )
    {
        _houseBrokerDbContext = houseBrokerDbContext;
    }

    public async Task<IEnumerable<Commission>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _houseBrokerDbContext.Commissions.ToListAsync(cancellationToken);
    }
}