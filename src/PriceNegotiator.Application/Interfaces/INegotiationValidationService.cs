using PriceNegotiator.Domain.Entities.Negotiations;

namespace PriceNegotiator.Application.Interfaces;

public interface INegotiationValidationService
{
    Task ValidateNewAttemptAsync(Negotiation negotiation);
}