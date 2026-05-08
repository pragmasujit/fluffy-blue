using HouseBroker.Application.Providers.CommissionProvider;

namespace HouseBroker.Application.Services;

public class CommissionEngine : ICommissionEngine
{
    private readonly ICommissionProvider _ruleProvider;

    public CommissionEngine(ICommissionProvider ruleProvider)
    {
        _ruleProvider = ruleProvider;
    }

    public async Task<decimal> CalculateCommissionAsync(decimal price, CancellationToken cancellationToken)
    {
        var rules = await _ruleProvider.GetRulesAsync(cancellationToken);

        var rule = rules.FirstOrDefault(r =>
            price >= r.MinPrice &&
            (r.MaxPrice == null || price <= r.MaxPrice));

        if (rule is null)
            throw new Exception("No commission rule found");

        return price * rule.Percentage;
    }
}