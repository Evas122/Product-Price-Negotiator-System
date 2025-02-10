using PriceNegotiator.Domain.Entities.Negotiations;

namespace PriceNegotiator.Domain.Interfaces;

public interface INegotiationValidationService
{
    Task ValidateNewAttemptAsync(Negotiation negotiation);
}