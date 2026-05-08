namespace HouseBroker.Domain.Misc.Isos;

public static class IsoCurrencies
{
    public static readonly (string Code, string Symbol, string Name) UnitedStatesDollar = ("USD", "$", "United States Dollar");
    public static readonly (string Code, string Symbol, string Name) NepaleseRupee = ("NPR", "रु", "Nepali Rupee");

    public static readonly IEnumerable<(string Code, string Symbol, string Name)> All = new[]
    {
        UnitedStatesDollar, NepaleseRupee
    };
}
