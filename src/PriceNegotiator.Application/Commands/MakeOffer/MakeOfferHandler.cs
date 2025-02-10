using MediatR;
using PriceNegotiator.Domain.Common.Exceptions.Base;
using PriceNegotiator.Domain.Interfaces;
using PriceNegotiator.Domain.Interfaces.Messaging;
using PriceNegotiator.Domain.Entities.Negotiations;
using PriceNegotiator.Domain.Enums;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Domain.Commands.MakeOffer;

public record MakeOfferCommand(string ClientEmail, Guid ProductId, decimal ProposedPrice) : ICommand<Unit>;

public class MakeOfferHandler : ICommandHandler<MakeOfferCommand, Unit>
{
    private readonly INegotiationRepository _negotiationRepository;
    private readonly INegotiationValidationService _negotiationValidationService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IProductRepository _productRepository;

    public MakeOfferHandler(INegotiationRepository negotiationRepository,
        INegotiationValidationService negotiationValidationService,
        IDateTimeProvider dateTimeProvider,
        IProductRepository productRepository)
    {
        _negotiationRepository = negotiationRepository;
        _negotiationValidationService = negotiationValidationService;
        _dateTimeProvider = dateTimeProvider;
        _productRepository = productRepository;
    }

    public async Task<Unit> Handle(MakeOfferCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.ExistAsync(command.ProductId);
        if (!product)
        {
            throw new NotFoundException(nameof(product), command.ProductId.ToString());
        }

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

    private Negotiation CreateNewNegotiation(MakeOfferCommand command)
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