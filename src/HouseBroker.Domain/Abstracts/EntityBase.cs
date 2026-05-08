namespace HouseBroker.Domain.Abstracts;

public abstract class EntityBase
{
    public int Id { get; protected set; }
    public Guid Guid { get; protected set; }
}