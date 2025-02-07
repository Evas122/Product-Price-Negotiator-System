using PriceNegotiator.Domain.Entities.Negotiations;

namespace PriceNegotiator.Domain.Repositories;

public interface INegotiationRepository
{
    Task<Negotiation?> GetByClientEmailandProductIdAsync(string clientEmail, Guid productId);
    Task AddAsync(Negotiation negotiation);
    Task UpdateAsync(Negotiation negotiation);
    Task<bool> ExistAsync(Guid negotiationId);
}