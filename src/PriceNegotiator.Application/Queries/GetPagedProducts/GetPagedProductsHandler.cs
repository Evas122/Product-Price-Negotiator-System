using PriceNegotiator.Application.Dtos.Assortment;
using PriceNegotiator.Application.Dtos.Paged;
using PriceNegotiator.Application.Extensions.Mappings.Products;
using PriceNegotiator.Application.Interfaces.Messaging;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Application.Queries.GetPagedProducts;

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