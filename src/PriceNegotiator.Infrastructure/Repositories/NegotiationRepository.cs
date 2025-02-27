﻿using Microsoft.EntityFrameworkCore;
using PriceNegotiator.Application.Interfaces;
using PriceNegotiator.Domain.Entities.Negotiations;
using PriceNegotiator.Domain.Enums;
using PriceNegotiator.Domain.Repositories;
using PriceNegotiator.Infrastructure.Data;

namespace PriceNegotiator.Infrastructure.Repositories;

public class NegotiationRepository : INegotiationRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IDomainEventDispatcher _eventDispatcher;

    public NegotiationRepository(AppDbContext dbContext, IDomainEventDispatcher eventDispatcher)
    {
        _dbContext = dbContext;
        _eventDispatcher = eventDispatcher;
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

    public async Task<Negotiation?> GetByIdAsync(Guid negotiationId)
    {
        return await _dbContext.Negotiations.FindAsync(negotiationId);
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

    public async Task<IEnumerable<Negotiation>> GetExpiredNegotiationsAsync(DateTime threshold)
    {
        return await _dbContext.Negotiations.
            Where(x => x.Status == NegotiationStatus.Rejected && x.LastRejectionAt.HasValue && x.LastRejectionAt.Value <= threshold)
            .ToListAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<Negotiation> negotiations)
    {
        _dbContext.Negotiations.UpdateRange(negotiations);

        var domainEvents = negotiations
            .SelectMany(n => n.DomainEvents)
            .ToList();

        await _dbContext.SaveChangesAsync();

        await _eventDispatcher.DispatchEventsAsync(domainEvents);

        foreach (var negotiation in negotiations)
        {
            negotiation.ClearDomainEvents();
        }
    }
}