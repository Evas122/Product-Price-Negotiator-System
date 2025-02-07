using PriceNegotiator.Domain.Entities.Negotiations;
using PriceNegotiator.Domain.Repositories;
using PriceNegotiator.Infrastructure.Data;

namespace PriceNegotiator.Infrastructure.Repositories;

public class NegotiationRepository : INegotiationRepository
{
    private readonly AppDbContext _dbContext;

    public NegotiationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddAsync(Negotiation negotiation)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistAsync(Guid negotiationId)
    {
        throw new NotImplementedException();
    }

    public Task<Negotiation> GetByClientEmailandProductIdAsync(string clientEmail, Guid productId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Negotiation negotiation)
    {
        throw new NotImplementedException();
    }
}