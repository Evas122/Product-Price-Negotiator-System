using Microsoft.EntityFrameworkCore;
using PriceNegotiator.Domain.Entities.Assortments;
using PriceNegotiator.Infrastructure.Data;
using PriceNegotiator.Infrastructure.Repositories;

namespace PriceNegotiator.Infrastructure.UnitTests.RepositoriesTests;

public class ProductRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldAddProductToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        using var context = new AppDbContext(options);
        var repository = new ProductRepository(context);
        var product = new Product { Id = Guid.NewGuid(), Description = "Test product desc", Name = "Test Product", BasePrice = 2.4m };

        // Act
        await repository.AddAsync(product);

        // Assert
        var savedProduct = await context.Products.FindAsync(product.Id);
        Assert.NotNull(savedProduct);
        Assert.Equal(product.Name, savedProduct.Name);
    }

    [Fact]
    public async Task GetPagedProductsAsync_ReturnsCorrectPageSize()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        using var context = new AppDbContext(options);
        var repository = new ProductRepository(context);

        // Add 15 products
        for (int i = 0; i < 15; i++)
        {
            context.Products.Add(new Product { Id = Guid.NewGuid(), Description = $"Product desc{i}", Name = $"Product {i}", BasePrice = 2.4m });
        }
        await context.SaveChangesAsync();

        // Act
        var (items, totalItems) = await repository.GetPagedProductsAsync(1, 10);

        // Assert
        Assert.Equal(10, items.Count());
        Assert.Equal(15, totalItems);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectProduct()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        using var context = new AppDbContext(options);
        var repository = new ProductRepository(context);
        var product = new Product { Id = Guid.NewGuid(), Description = "Test product desc", Name = "Test Product" };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(product.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Equal(product.Name, result.Name);
    }
}