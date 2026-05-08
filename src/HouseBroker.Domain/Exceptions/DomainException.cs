namespace HouseBroker.Domain.Exceptions;

public class DomainException: Exception
{
    public string Code { get; }
    public DomainException(
        string message
    ): base(message)
    {
        Code = "1";
    }
}