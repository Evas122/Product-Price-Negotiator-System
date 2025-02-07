using MediatR;
using PriceNegotiator.Application.Interfaces;
using PriceNegotiator.Application.Interfaces.Messaging;
using PriceNegotiator.Domain.Entities.Negotiations;
using PriceNegotiator.Domain.Enums;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Application.Commands.MakeOffer;

public record MakeNegotiationCommand(string ClientEmail, Guid ProductId, decimal ProposedPrice) : ICommand<Unit>;

public class MakeOfferHandler : ICommandHandler<MakeNegotiationCommand, Unit>
{
    private readonly INegotiationRepository _negotiationRepository;
    private readonly INegotiationValidationService _negotiationValidationService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MakeOfferHandler(INegotiationRepository negotiationRepository, INegotiationValidationService negotiationValidationService, IDateTimeProvider dateTimeProvider)
    {
        _negotiationRepository = negotiationRepository;
        _negotiationValidationService = negotiationValidationService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Unit> Handle(MakeNegotiationCommand command, CancellationToken cancellationToken)
    {
        var existingNegotiation = await _negotiationRepository.GetByClientEmailandProductIdAsync(command.ClientEmail, command.ProductId);

        if (existingNegotiation != null)
        {
            await _negotiationValidationService.ValidateNewAttemptAsync(existingNegotiation);

            var newAttempt = CreateNegotiationAttempt(existingNegotiation, command.ProposedPrice);
            existingNegotiation.NegotiationAttempts.Add(newAttempt);
            existingNegotiation.AttemptCount++;
            existingNegotiation.Status = NegotiationStatus.WaitingForEmployee;

            await _negotiationRepository.UpdateAsync(existingNegotiation);
        }
        else
        {
            var newNegotiation = CreateNewNegotiation(command);
            await _negotiationRepository.AddAsync(newNegotiation);
        }

        return Unit.Value;
    }

    private NegotiationAttempt CreateNegotiationAttempt(Negotiation negotiation, decimal proposedPrice)
    {
        var attemptNumber = negotiation.AttemptCount + 1;

        return new NegotiationAttempt
        {
            NegotiationId = negotiation.Id,
            ProposedPrice = proposedPrice,
            ProposedAt = _dateTimeProvider.UtcNow,
            AttemptNumber = attemptNumber,
            CreatedAt = _dateTimeProvider.UtcNow,
            UpdatedAt = _dateTimeProvider.UtcNow,
        };
    }

    private Negotiation CreateNewNegotiation(MakeNegotiationCommand command)
    {
        return new Negotiation
        {
            ProductId = command.ProductId,
            ClientEmail = command.ClientEmail,
            Status = NegotiationStatus.WaitingForEmployee,
            AttemptCount = 1,
            LastRejectionAt = null,
            NegotiationAttempts = new List<NegotiationAttempt>
            {
                new NegotiationAttempt
                {
                    ProposedPrice = command.ProposedPrice,
                    ProposedAt = _dateTimeProvider.UtcNow,
                    AttemptNumber = 1,
                    CreatedAt = _dateTimeProvider.UtcNow,
                    UpdatedAt = _dateTimeProvider.UtcNow
                }
            },
            CreatedAt = _dateTimeProvider.UtcNow,
            UpdatedAt = _dateTimeProvider.UtcNow
        };
    }
}