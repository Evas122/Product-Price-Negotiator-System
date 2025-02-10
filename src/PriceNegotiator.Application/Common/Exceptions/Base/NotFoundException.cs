namespace PriceNegotiator.Application.Common.Exceptions.Base;

public class NotFoundException : Exception
{
    public string ResourceType { get; }
    public string ResourceName { get; }

    public NotFoundException(string resourceType, string resourceName) : base($"{resourceType} with {resourceName} doesn't exist")
    {
        ResourceType = resourceType;
        ResourceName = resourceName;
    }
}