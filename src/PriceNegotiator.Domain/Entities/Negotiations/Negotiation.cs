using PriceNegotiator.Domain.Common;
using PriceNegotiator.Domain.Entities.Assortments;
using PriceNegotiator.Domain.Enums;

namespace PriceNegotiator.Domain.Entities.Negotiations;

public class Negotiation : BaseEntity
{
    public Guid ProductId { get; set; }
    public string ClientEmail { get; set; } = null!;
    public NegotiationStatus Status { get; set; }
    public int AttemptCount { get; set; }
    public DateTime? LastRejectionAt { get; set; }
    public Product Product { get; set; } = null!;
    public ICollection<NegotiationAttempt> NegotiationAttempts { get; set; } = []; 
}