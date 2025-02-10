using MediatR;
using PriceNegotiator.Domain.Common.Interfaces;

namespace PriceNegotiator.Domain.Common.DomainEvents;

public class DomainEventNotification<TDomainEvent> : INotification
    where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; }

    public DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}