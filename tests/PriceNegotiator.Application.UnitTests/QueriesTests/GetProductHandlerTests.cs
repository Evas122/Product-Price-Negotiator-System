using Moq;
using PriceNegotiator.Application.Common.Exceptions.Base;
using PriceNegotiator.Application.Queries.GetProduct;
using PriceNegotiator.Domain.Entities.Assortments;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Application.UnitTests.QueriesTests;

public class GetProductHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly GetProductHandler _handler;

    public GetProductHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _handler = new GetProductHandler(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingProduct_ReturnsCorrectProductDto()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Name = "Test Product",
            Description = "Test Description",
            BasePrice = 100m
        };
        var query = new GetProductQuery(productId);

        _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
            .ReturnsAsync(product);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        Assert.Equal(product.Name, result.Name);
        Assert.Equal(product.Description, result.Description);
        Assert.Equal(product.BasePrice, result.BasePrice);
    }

    [Fact]
    public async Task Handle_NonExistingProduct_ThrowsNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var query = new GetProductQuery(productId);

        _productRepositoryMock.Setup(x => x.GetByIdAsync(productId))
            .ReturnsAsync((Product)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(query, CancellationToken.None));
    }
}