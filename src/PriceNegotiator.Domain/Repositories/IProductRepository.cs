using PriceNegotiator.Domain.Entities.Assortments;

namespace PriceNegotiator.Domain.Repositories;

public interface IProductRepository
{
    Task AddProductAsync(Product product);
    Task<Product?> GetProductByIdAsync(Guid productId);
    Task<(IEnumerable<Product> Items, int TotalItems)> GetPagedProducts(int pageNumber, int pageSize);
}