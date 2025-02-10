using FluentValidation;

namespace PriceNegotiator.Domain.Commands.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("The product name cannot be empty");

        RuleFor(p => p.Description)
            .NotEmpty()
            .WithMessage("The product description cannot be empty");

        RuleFor(p => p.BasePrice)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Base Price must be greater than 0 and a valid number.");
    }
}