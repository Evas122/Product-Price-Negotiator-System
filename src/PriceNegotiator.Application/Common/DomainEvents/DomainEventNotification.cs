using MediatR;
using PriceNegotiator.Domain.Common.Interfaces;

namespace PriceNegotiator.Application.Common.DomainEvents;

public class DomainEventNotification<TDomainEvent> : INotification
    where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; }

    public DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}