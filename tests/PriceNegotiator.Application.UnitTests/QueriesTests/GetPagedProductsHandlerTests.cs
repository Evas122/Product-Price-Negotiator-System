using Moq;
using PriceNegotiator.Domain.Queries.GetPagedProducts;
using PriceNegotiator.Domain.Entities.Assortments;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Domain.UnitTests.QueriesTests;

public class GetPagedProductsHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly GetPagedProductsHandler _handler;

    public GetPagedProductsHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _handler = new GetPagedProductsHandler(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithDefaultParameters_UsesCorrectDefaultValues()
    {
        // Arrange
        var defaultPage = 1;
        var defaultPageSize = 10;
        var query = new GetPagedProductsQuery(null, null);
        var products = new List<Product>();
        var totalItems = 0;

        _productRepositoryMock.Setup(x => x.GetPagedProductsAsync(defaultPage, defaultPageSize))
            .ReturnsAsync((products, totalItems));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalItemsCount);
        Assert.Equal(0, result.TotalPages);
        Assert.Equal(0, result.ItemsFrom);
        Assert.Equal(0, result.ItemsTo);
    }

    [Fact]
    public async Task Handle_VerifyProductDtoMapping()
    {
        // Arrange
        var page = 1;
        var pageSize = 10;
        var productId = Guid.NewGuid();
        var query = new GetPagedProductsQuery(page, pageSize);

        var products = new List<Product>
        {
            new Product
            {
                Id = productId,
                Name = "Test Product",
                BasePrice = 199.99m
            }
        };

        _productRepositoryMock.Setup(x => x.GetPagedProductsAsync(page, pageSize))
            .ReturnsAsync((products, 1));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var productDto = result.Items.First();
        Assert.Equal(productId, productDto.Id);
        Assert.Equal("Test Product", productDto.Name);
        Assert.Equal(199.99m, productDto.BasePrice);
    }
}