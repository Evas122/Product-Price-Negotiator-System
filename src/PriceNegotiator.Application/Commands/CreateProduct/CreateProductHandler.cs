using MediatR;
using PriceNegotiator.Application.Interfaces;
using PriceNegotiator.Application.Interfaces.Messaging;
using PriceNegotiator.Domain.Entities.Assortment;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Application.Commands.CreateProduct;

public record CreateProductCommand(string Name, string Description, decimal BasePrice) : ICommand<Unit>;

public class CreateProductHandler : ICommandHandler<CreateProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateProductHandler(IProductRepository productRepository, IDateTimeProvider dateTimeProvider)
    {
        _productRepository = productRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Unit> Handle (CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = command.Name,
            Description = command.Description,
            BasePrice = command.BasePrice,
            CreatedAt = _dateTimeProvider.UtcNow,
            UpdatedAt = _dateTimeProvider.UtcNow,
        };
        await _productRepository.AddProductAsync(product);

        return Unit.Value;
    }

}