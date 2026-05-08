using HouseBroker.Domain.Contexts.HouseBroker.Commission;

namespace HouseBroker.Application.Services;

public interface ICommissionEngine
{
    Task<decimal> CalculateCommissionAsync(decimal price, CancellationToken cancellationToken);
}