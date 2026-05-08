namespace HouseBroker.Domain.Abstracts;

public class AuditableEntity: EntityBase
{
    public DateTime CreatedAt { get; protected set; }
    public DateTime UpdatedAt { get; protected set; }
    public string? CreatedBy { get; protected set; }
    public string? UpdatedBy { get; protected set; }
}