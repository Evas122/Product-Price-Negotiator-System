using PriceNegotiator.Domain.Common.Interfaces;

namespace PriceNegotiator.Application.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync(IEnumerable<IDomainEvent> events);
}