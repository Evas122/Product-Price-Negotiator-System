using FluentValidation;

namespace PriceNegotiator.Domain.Commands.MakeOffer;

public class MakeOfferValidator : AbstractValidator<MakeOfferCommand>
{
    public MakeOfferValidator()
    {
        RuleFor(x => x.ClientEmail)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("CLient Email cannot be empty, and must be correct email address.");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product id is required");

        RuleFor(x => x.ProposedPrice)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Proposed price is required and must be greather than 0");
    }
}