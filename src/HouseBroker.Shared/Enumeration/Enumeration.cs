namespace HouseBroker.Shared.Enumeration;

public abstract class Enumeration : IComparable
{
    public string Name { get; }
    public int Value { get; }

    protected Enumeration(int value, string name)
    {
        Value = value;
        Name = name;
    }

    public override string ToString() => Name;

    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(System.Reflection.BindingFlags.Public |
                            System.Reflection.BindingFlags.Static |
                            System.Reflection.BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
            return false;

        return Value == otherValue.Value && GetType() == otherValue.GetType();
    }

    public override int GetHashCode() => (GetType().ToString() + Value).GetHashCode();

    public int CompareTo(object? other) => Value.CompareTo(((Enumeration)other!).Value);
}
