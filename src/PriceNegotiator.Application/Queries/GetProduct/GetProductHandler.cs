using PriceNegotiator.Application.Dtos.Assortment;
using PriceNegotiator.Application.Extensions.Mappings.Products;
using PriceNegotiator.Application.Interfaces.Messaging;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Application.Queries.GetProduct;

public record GetProductQuery(Guid Id) : IQuery<ProductDto>;

public class GetProductHandler : IQueryHandler<GetProductQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public GetProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(query.Id) ?? throw new ApplicationException("product not found");

        return product.ToDto();
    }
}