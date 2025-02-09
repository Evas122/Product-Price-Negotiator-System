using Moq;
using PriceNegotiator.Application.Commands.MakeOffer;
using PriceNegotiator.Application.Common.Exceptions.Base;
using PriceNegotiator.Application.Interfaces;
using PriceNegotiator.Domain.Entities.Negotiations;
using PriceNegotiator.Domain.Enums;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Application.UnitTests.CommandsTests;

public class MakeOfferHandlerTests
{
    private readonly Mock<INegotiationRepository> _negotiationRepositoryMock;
    private readonly Mock<INegotiationValidationService> _negotiationValidationServiceMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly MakeOfferHandler _handler;

    public MakeOfferHandlerTests()
    {
        _negotiationRepositoryMock = new Mock<INegotiationRepository>();
        _negotiationValidationServiceMock = new Mock<INegotiationValidationService>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _handler = new MakeOfferHandler(
            _negotiationRepositoryMock.Object,
            _negotiationValidationServiceMock.Object,
            _dateTimeProviderMock.Object,
            _productRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithNewNegotiation_CreatesNewNegotiation()
    {
        // Arrange
        var command = new MakeOfferCommand("test@test.com", Guid.NewGuid(), 100m);
        var currentTime = DateTime.UtcNow;

        _productRepositoryMock.Setup(x => x.ExistAsync(command.ProductId))
            .ReturnsAsync(true);
        _negotiationRepositoryMock.Setup(x => x.GetByClientEmailandProductIdAsync(command.ClientEmail, command.ProductId))
            .ReturnsAsync((Negotiation)null);
        _dateTimeProviderMock.Setup(x => x.UtcNow)
            .Returns(currentTime);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _negotiationRepositoryMock.Verify(x => x.AddAsync(
            It.Is<Negotiation>(n =>
                n.ClientEmail == command.ClientEmail &&
                n.ProductId == command.ProductId &&
                n.Status == NegotiationStatus.WaitingForEmployee &&
                n.AttemptCount == 1)), Times.Once);
    }

    [Fact]
    public async Task Handle_WithExistingNegotiation_AddsNewAttempt()
    {
        // Arrange
        var command = new MakeOfferCommand("test@test.com", Guid.NewGuid(), 100m);
        var existingNegotiation = new Negotiation
        {
            Id = Guid.NewGuid(),
            ClientEmail = command.ClientEmail,
            ProductId = command.ProductId,
            AttemptCount = 1,
            NegotiationAttempts = new List<NegotiationAttempt>()
        };

        _productRepositoryMock.Setup(x => x.ExistAsync(command.ProductId))
            .ReturnsAsync(true);
        _negotiationRepositoryMock.Setup(x => x.GetByClientEmailandProductIdAsync(command.ClientEmail, command.ProductId))
            .ReturnsAsync(existingNegotiation);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _negotiationRepositoryMock.Verify(x => x.UpdateAsync(
            It.Is<Negotiation>(n =>
                n.AttemptCount == 2 &&
                n.Status == NegotiationStatus.WaitingForEmployee)), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentProduct_ThrowsNotFoundException()
    {
        // Arrange
        var command = new MakeOfferCommand("test@test.com", Guid.NewGuid(), 100m);

        _productRepositoryMock.Setup(x => x.ExistAsync(command.ProductId))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}