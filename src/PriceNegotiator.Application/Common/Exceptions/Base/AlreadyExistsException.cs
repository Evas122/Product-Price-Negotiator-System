namespace PriceNegotiator.Application.Common.Exceptions.Base;

public class AlreadyExistsException : Exception
{
    public string ExistsValue { get; }

    public AlreadyExistsException(string existsValue)
        : base($"The value '{existsValue}' already exists.")
    {
        ExistsValue = existsValue;
    }

    public AlreadyExistsException(string existsValue, Exception innerException)
        : base($"The value '{existsValue}' already exists.", innerException)
    {
        ExistsValue = existsValue;
    }
}