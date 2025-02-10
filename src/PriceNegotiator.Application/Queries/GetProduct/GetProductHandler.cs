using PriceNegotiator.Domain.Common.Exceptions.Base;
using PriceNegotiator.Domain.Dtos.Assortment;
using PriceNegotiator.Domain.Extensions.Mappings.Products;
using PriceNegotiator.Domain.Interfaces.Messaging;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Domain.Queries.GetProduct;

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
        var product = await _productRepository.GetByIdAsync(query.Id);
        if (product == null)
        {
            throw new NotFoundException(nameof(product), query.Id.ToString());
        }
        return product.ToDto();
    }
}