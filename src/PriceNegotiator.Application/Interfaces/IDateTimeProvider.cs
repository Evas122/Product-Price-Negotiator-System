namespace PriceNegotiator.Domain.Interfaces;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}