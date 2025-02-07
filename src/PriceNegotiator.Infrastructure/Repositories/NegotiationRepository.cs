using Microsoft.EntityFrameworkCore;
using PriceNegotiator.Domain.Entities.Negotiations;
using PriceNegotiator.Domain.Enums;
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
        _dbContext.Negotiations.Add(negotiation);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistAsync(Guid negotiationId)
    {
        return await _dbContext.Negotiations.AnyAsync(x => x.Id == negotiationId);
    }

    public async Task<Negotiation?> GetByClientEmailandProductIdAsync(string clientEmail, Guid productId)
    {
        return await _dbContext.Negotiations.FirstOrDefaultAsync(x => x.ClientEmail == clientEmail && x.ProductId == productId);
    }

    public async Task UpdateAsync(Negotiation negotiation)
    {
        _dbContext.Negotiations.Update(negotiation);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(Guid negotiationId, NegotiationStatus newStatus)
    {
        var negotiation = await _dbContext.Negotiations.FindAsync(negotiationId);

        if (negotiation != null)
        {
            negotiation.Status = newStatus;
            _dbContext.Negotiations.Update(negotiation);
            await _dbContext.SaveChangesAsync();
        }
    }
}