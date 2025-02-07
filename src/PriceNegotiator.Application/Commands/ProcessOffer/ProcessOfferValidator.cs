using FluentValidation;

namespace PriceNegotiator.Application.Commands.ProcessOffer;

public class ProcessOfferValidator : AbstractValidator<ProcessOfferCommand>
{
    public ProcessOfferValidator()
    {
        RuleFor(x => x.NegotiationId)
            .NotEmpty()
            .WithMessage("Negotiation Id cannot be empty");

        RuleFor(x => x.Action)
            .NotEmpty()
            .IsInEnum()
            .WithMessage("Action should be correct enum");
    }
}