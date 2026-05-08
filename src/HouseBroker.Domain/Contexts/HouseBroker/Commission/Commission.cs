using HouseBroker.Domain.Exceptions;

namespace HouseBroker.Domain.Contexts.HouseBroker.Commission;

public class Commission
{
    public int Id { get; private set; }
    public decimal MinPrice { get; private set; }
    public decimal? MaxPrice { get; private set; }
    public decimal Percentage { get; private set; }

    private Commission() { }

    private Commission(decimal minPrice, decimal? maxPrice, decimal percentage)
    {
        MinPrice = minPrice;
        MaxPrice = maxPrice;
        Percentage = percentage;
    }

    public static Commission Create(decimal minPrice, decimal? maxPrice, decimal percentage)
    {
        if (minPrice < 0)
            throw new DomainException("MinPrice cannot be negative");

        if (percentage <= 0)
            throw new DomainException("Percentage must be greater than 0");

        if (maxPrice.HasValue && maxPrice < minPrice)
            throw new DomainException("MaxPrice cannot be less than MinPrice");

        return new Commission(minPrice, maxPrice, percentage);
    }
    
    public static Commission Hydrate(int id, decimal minPrice, decimal? maxPrice, decimal percentage)
    {
        return new Commission(minPrice, maxPrice, percentage)
        {
            Id = id
        };
    }
}