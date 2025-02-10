using FluentValidation;

namespace PriceNegotiator.Domain.Queries.GetPagedProducts;

public class GetPagedProductsValidator : AbstractValidator<GetPagedProductsQuery>
{
    public GetPagedProductsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page number cannot be lower than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(10)
            .WithMessage("Page size cannot be lower than 0");
    }
}