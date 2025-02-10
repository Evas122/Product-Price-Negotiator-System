using PriceNegotiator.Domain.Common.Interfaces;

namespace PriceNegotiator.Domain.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync(IEnumerable<IDomainEvent> events);
}