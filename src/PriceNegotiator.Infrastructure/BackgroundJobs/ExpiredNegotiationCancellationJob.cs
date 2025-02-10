using Microsoft.Extensions.Logging;
using PriceNegotiator.Domain.Interfaces;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Infrastructure.BackgroundJobs;

public class ExpiredNegotiationCancellationJob
{
    private readonly INegotiationRepository _negotiationRepository;
    private readonly ILogger<ExpiredNegotiationCancellationJob> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ExpiredNegotiationCancellationJob( INegotiationRepository negotiationRepository,
        ILogger<ExpiredNegotiationCancellationJob> logger,
        IDateTimeProvider dateTimeProvider)
    {
        _negotiationRepository = negotiationRepository;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Execute()
    {
        _logger.LogInformation("Hangfire Job: Checking for expired negotiations at {Time}", _dateTimeProvider.UtcNow);

        var thresholdDate = _dateTimeProvider.UtcNow.AddDays(-7);
        var expiredNegotiations = await _negotiationRepository.GetExpiredNegotiationsAsync(thresholdDate);

        if (expiredNegotiations.Any())
        {
            foreach (var negotiation in expiredNegotiations)
            {
                negotiation.Cancel();
            }

            await _negotiationRepository.UpdateRangeAsync(expiredNegotiations);
            _logger.LogInformation("Cancelled {Count} negotiations.", expiredNegotiations.Count());
        }
        else
        {
            _logger.LogInformation("No expired negotiations found.");
        }
    }
}