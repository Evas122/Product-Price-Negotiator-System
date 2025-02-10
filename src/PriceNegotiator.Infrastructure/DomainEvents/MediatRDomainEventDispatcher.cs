using MediatR;
using PriceNegotiator.Domain.Common.DomainEvents;
using PriceNegotiator.Domain.Interfaces;
using PriceNegotiator.Domain.Common.Interfaces;

namespace PriceNegotiator.Infrastructure.DomainEvents;

public class MediatRDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public MediatRDomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchEventsAsync(IEnumerable<IDomainEvent> events)
    {
        foreach (var @event in events)
        {
            var notificationType = typeof(DomainEventNotification<>).MakeGenericType(@event.GetType());
            var notification = Activator.CreateInstance(notificationType, @event)
                ?? throw new InvalidOperationException($"Failed to create notification for event type: {@event.GetType().Name}");
            await _mediator.Publish(notification);
        }
    }
}