﻿using PriceNegotiator.Application.Interfaces;
using PriceNegotiator.Domain.Entities.Negotiations;
using PriceNegotiator.Domain.Enums;

namespace PriceNegotiator.Infrastructure.Services;

public class NegotiationValidationService : INegotiationValidationService
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public NegotiationValidationService(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public Task ValidateNewAttemptAsync(Negotiation negotiation)
    {
        ValidateFinalStatuses(negotiation);
        ValidateAttemptLimit(negotiation);
        ValidateRejectionPeriod(negotiation);

        return Task.CompletedTask;
    }

    private void ValidateFinalStatuses(Negotiation negotiation)
    {
        switch (negotiation.Status)
        {
            case NegotiationStatus.Cancelled:
                throw new ApplicationException("Negotiations have been canceled due to time limit 7 days. You cannot make another offer.");

            case NegotiationStatus.AttemptsExceeded:
                throw new ApplicationException("Limit of negotiation attempts has been reached.");

            case NegotiationStatus.Accepted:
                throw new ApplicationException("Negotiations have been finished. Offer has been accepted.");

            case NegotiationStatus.WaitingForEmployee:
                throw new ApplicationException("The offer is pending confirmation by an employee.");
        }
    }

    private void ValidateAttemptLimit(Negotiation negotiation)
    {
        if (negotiation.NegotiationAttempts.Count >= 3)
        {
            negotiation.Status = NegotiationStatus.AttemptsExceeded;
            throw new ApplicationException("Limit of negotiation attempts has been reached.");
        }
    }

    private void ValidateRejectionPeriod(Negotiation negotiation)
    {
        if (negotiation.Status != NegotiationStatus.Rejected)
            return;

        if (!negotiation.LastRejectionAt.HasValue)
            return;

        if (_dateTimeProvider.UtcNow < negotiation.LastRejectionAt.Value.AddDays(7))
            return;

        negotiation.Status = NegotiationStatus.Cancelled;
        throw new ApplicationException("The 7-day period for resubmitting an offer has expired.");
    }
}