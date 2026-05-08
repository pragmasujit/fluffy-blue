using HouseBroker.Application.Repositories;
using HouseBroker.Domain.Contexts.HouseBroker.Commission;
using Microsoft.Extensions.Caching.Memory;

namespace HouseBroker.Application.Providers.CommissionProvider;

public class CommissionProvider : ICommissionProvider
{
    private const string CacheKey = "commission_rules";

    private readonly ICommissionWriteRepository _repository;
    private readonly IMemoryCache _cache;

    public CommissionProvider(
        ICommissionWriteRepository repository,
        IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<IEnumerable<Commission>> GetRulesAsync(CancellationToken cancellationToken)
    {
        return await _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return await _repository.GetAllAsync(cancellationToken);
        }) ?? new List<Commission>();
    }
}