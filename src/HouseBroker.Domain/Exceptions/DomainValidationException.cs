namespace HouseBroker.Domain.Exceptions;

public class DomainValidationException: DomainException
{
    public string Identifier { get; }

    public DomainValidationException(
        string identifier,
        string message)
    :base(
        message: message
    )
    {
        Identifier = identifier;
    }
}