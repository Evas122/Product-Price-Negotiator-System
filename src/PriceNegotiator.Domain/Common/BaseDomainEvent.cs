using PriceNegotiator.Domain.Common.Interfaces;

namespace PriceNegotiator.Domain.Common;

public abstract class BaseDomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; }

    protected BaseDomainEvent()
    {
        OccurredOn = DateTime.UtcNow;
    }
}