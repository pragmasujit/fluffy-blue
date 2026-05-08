using HouseBroker.Application.Repositories.Abstracts;
using HouseBroker.Infrastructure.Data;

namespace HouseBroker.Application.UnitOfWork;

public class HouseBrokerUnitOfWork: IHouseBrokerUnitOfWork
{
    private readonly HouseBrokerDbContext _dbContext;

    public HouseBrokerUnitOfWork(HouseBrokerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}