using MediatR;
using PriceNegotiator.Application.Common.Exceptions.Base;
using PriceNegotiator.Application.Interfaces;
using PriceNegotiator.Application.Interfaces.Messaging;
using PriceNegotiator.Domain.Enums;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Application.Commands.ProcessOffer;

public record ProcessOfferCommand(Guid NegotiationId, EmployeeAction Action) : ICommand<Unit>;

public class ProcessOfferHandler : ICommandHandler<ProcessOfferCommand, Unit>
{
    private readonly INegotiationRepository _negotiationRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ProcessOfferHandler(INegotiationRepository negotiationRepository, IDateTimeProvider dateTimeProvider)
    {
        _negotiationRepository = negotiationRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Unit> Handle(ProcessOfferCommand command, CancellationToken cancellationToken)
    {
        var negotiation = await _negotiationRepository.GetByIdAsync(command.NegotiationId);

        if (negotiation == null)
        {
            throw new NotFoundException(nameof(negotiation), command.NegotiationId.ToString());
        }

        if (negotiation.Status != NegotiationStatus.WaitingForEmployee)
        {
            throw new BadRequestException("Only negotiations waiting for employee confirmation can be processed.");
        }

        switch (command.Action)
        {
            case EmployeeAction.Accept:
                negotiation.Status = NegotiationStatus.Accepted;
                break;

            case EmployeeAction.Reject:
                negotiation.Status = NegotiationStatus.Rejected;
                negotiation.LastRejectionAt = _dateTimeProvider.UtcNow;
                negotiation.UpdatedAt = _dateTimeProvider.UtcNow;
                break;

            default:
                throw new BadRequestException("Invalid employee action specified.");
        }

        await _negotiationRepository.UpdateAsync(negotiation);

        return Unit.Value;
    }
}