using PriceNegotiator.Domain.Common;

namespace PriceNegotiator.Domain.Events;

public class NegotiationCancelledEvent : BaseDomainEvent
{
    public Guid NegotiationId { get; }
    public string ClientEmail { get; }

    public NegotiationCancelledEvent(Guid negotiationId, string clientEmail)
    {
        NegotiationId = negotiationId;
        ClientEmail = clientEmail;
    }
}