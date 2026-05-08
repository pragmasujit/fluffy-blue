using HouseBroker.Domain.Contexts.HouseBroker.Commission;

namespace HouseBroker.Application.Providers.CommissionProvider;

public interface ICommissionProvider
{
    Task<IEnumerable<Commission>> GetRulesAsync(CancellationToken cancellationToken);
}