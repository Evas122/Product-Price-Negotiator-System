using PriceNegotiator.Domain.Entities.Negotiations;
using PriceNegotiator.Domain.Enums;

namespace PriceNegotiator.Domain.Repositories;

public interface INegotiationRepository
{
    Task<Negotiation?> GetByClientEmailandProductIdAsync(string clientEmail, Guid productId);
    Task AddAsync(Negotiation negotiation);
    Task UpdateAsync(Negotiation negotiation);
    Task<bool> ExistAsync(Guid negotiationId);
    Task UpdateStatusAsync(Guid negotiationId, NegotiationStatus newStatus);
}