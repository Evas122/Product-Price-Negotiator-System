using FluentValidation;

namespace PriceNegotiator.Application.Queries.GetProduct;

public class GetProductValidator : AbstractValidator<GetProductQuery>
{
    public GetProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product Id cannot be empty.");
    }
}