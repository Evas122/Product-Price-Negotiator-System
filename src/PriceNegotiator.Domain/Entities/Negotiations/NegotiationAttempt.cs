using PriceNegotiator.Domain.Common;

namespace PriceNegotiator.Domain.Entities.Negotiations;

public class NegotiationAttempt : BaseEntity
{
    public Guid NegotiationId { get; set; }
    public decimal ProposedPrice { get; set; }
    public DateTime ProposedAt { get; set; }
    public int AttemptNumber { get; set; }
    public Negotiation Negotiation { get; set; } = null!;
}