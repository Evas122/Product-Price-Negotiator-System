using PriceNegotiator.Domain.Dtos.Assortment;
using PriceNegotiator.Domain.Dtos.Paged;
using PriceNegotiator.Domain.Extensions.Mappings.Products;
using PriceNegotiator.Domain.Interfaces.Messaging;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Domain.Queries.GetPagedProducts;

public record GetPagedProductsQuery(int? Page, int? PageSize) : IQuery<PagedDto<ProductDto>>;

public class GetPagedProductsHandler : IQueryHandler<GetPagedProductsQuery,PagedDto<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetPagedProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PagedDto<ProductDto>> Handle(GetPagedProductsQuery query, CancellationToken cancellationToken)
    {
        var page = query.Page ?? 1;
        var pageSize = query.PageSize ?? 10;

        var pagedProducts = await _productRepository.GetPagedProductsAsync(page, pageSize);

        return pagedProducts.ToPagedDto(page, pageSize);
    }
}