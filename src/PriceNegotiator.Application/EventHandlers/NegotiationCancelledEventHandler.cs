using MediatR;
using Microsoft.Extensions.Logging;
using PriceNegotiator.Application.Common.DomainEvents;
using PriceNegotiator.Domain.Events;

namespace PriceNegotiator.Application.EventHandlers;

public class NegotiationCancelledEventHandler : INotificationHandler<DomainEventNotification<NegotiationCancelledEvent>>
{
    private readonly ILogger<NegotiationCancelledEventHandler> _logger;

    public NegotiationCancelledEventHandler(ILogger<NegotiationCancelledEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        DomainEventNotification<NegotiationCancelledEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "Negotiation {NegotiationId} for client {ClientEmail} was cancelled.",
            domainEvent.NegotiationId,
            domainEvent.ClientEmail);

        return Task.CompletedTask;
    }
}