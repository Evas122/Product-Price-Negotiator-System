using Moq;
using PriceNegotiator.Domain.Commands.CreateProduct;
using PriceNegotiator.Domain.Interfaces;
using PriceNegotiator.Domain.Entities.Assortments;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Domain.UnitTests.CommandsTests;

public class CreateProductHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _handler = new CreateProductHandler(_productRepositoryMock.Object, _dateTimeProviderMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateProduct()
    {
        // Arrange
        var command = new CreateProductCommand("Test Product", "Test Description", 100m);
        var currentTime = DateTime.UtcNow;
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(currentTime);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _productRepositoryMock.Verify(x => x.AddAsync(It.Is<Product>(p =>
            p.Name == command.Name &&
            p.Description == command.Description &&
            p.BasePrice == command.BasePrice &&
            p.CreatedAt == currentTime &&
            p.UpdatedAt == currentTime)), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_VerifyTimeStamps()
    {
        // Arrange
        var command = new CreateProductCommand("Test Product", "Test Description", 100m);
        var currentTime = DateTime.UtcNow;
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(currentTime);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _productRepositoryMock.Verify(x => x.AddAsync(It.Is<Product>(p =>
            p.CreatedAt == currentTime && p.UpdatedAt == currentTime)), Times.Once);
    }
}