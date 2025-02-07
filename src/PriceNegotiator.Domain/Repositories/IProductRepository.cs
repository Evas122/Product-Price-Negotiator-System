using PriceNegotiator.Domain.Entities.Assortments;

namespace PriceNegotiator.Domain.Repositories;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<Product?> GetByIdAsync(Guid productId);
    Task<(IEnumerable<Product> Items, int TotalItems)> GetPagedProductsAsync(int pageNumber, int pageSize);
    Task<bool> ExistAsync(Guid productId);
}