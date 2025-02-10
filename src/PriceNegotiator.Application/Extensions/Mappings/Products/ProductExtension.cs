using PriceNegotiator.Domain.Dtos.Assortment;
using PriceNegotiator.Domain.Dtos.Paged;
using PriceNegotiator.Domain.Entities.Assortments;

namespace PriceNegotiator.Domain.Extensions.Mappings.Products;
public static class ProductExtension
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(product.Id, product.Name, product.Description, product.BasePrice);
    }

    public static PagedDto<ProductDto> ToPagedDto(this (IEnumerable<Product> Items, int TotalItems) pagedProducts, int pageSize, int pageNumber)
    {
        var productDtos = pagedProducts.Items
            .Select(product => new ProductDto(product.Id, product.Name, product.Description, product.BasePrice))
            .ToList();

        return new PagedDto<ProductDto>(
            items: productDtos,
            totalItems: pagedProducts.TotalItems,
            pageSize: pageSize,
            pageNumber: pageNumber
        );
    }
}