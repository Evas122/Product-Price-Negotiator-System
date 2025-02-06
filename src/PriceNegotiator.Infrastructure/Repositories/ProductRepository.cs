using Microsoft.EntityFrameworkCore;
using PriceNegotiator.Domain.Entities.Assortment;
using PriceNegotiator.Domain.Repositories;
using PriceNegotiator.Infrastructure.Data;

namespace PriceNegotiator.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddProductAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

    }

    public async Task<(IEnumerable<Product> Items, int TotalItems)> GetPagedProducts(int pageNumber, int pageSize)
    {
        var query = _dbContext.Products
            .OrderByDescending(t => t.CreatedAt);

        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalItems);
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
    }
}