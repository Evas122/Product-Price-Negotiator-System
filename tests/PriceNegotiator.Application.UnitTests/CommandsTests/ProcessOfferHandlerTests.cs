using Moq;
using PriceNegotiator.Domain.Commands.ProcessOffer;
using PriceNegotiator.Domain.Common.Exceptions.Base;
using PriceNegotiator.Domain.Interfaces;
using PriceNegotiator.Domain.Entities.Negotiations;
using PriceNegotiator.Domain.Enums;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Domain.UnitTests.CommandsTests;

public class ProcessOfferHandlerTests
{
    private readonly Mock<INegotiationRepository> _negotiationRepositoryMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly ProcessOfferHandler _handler;

    public ProcessOfferHandlerTests()
    {
        _negotiationRepositoryMock = new Mock<INegotiationRepository>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _handler = new ProcessOfferHandler(_negotiationRepositoryMock.Object, _dateTimeProviderMock.Object);
    }

    [Fact]
    public async Task Handle_AcceptOffer_UpdatesStatusToAccepted()
    {
        // Arrange
        var negotiationId = Guid.NewGuid();
        var command = new ProcessOfferCommand(negotiationId, EmployeeAction.Accept);
        var negotiation = new Negotiation { Id = negotiationId, Status = NegotiationStatus.WaitingForEmployee };

        _negotiationRepositoryMock.Setup(x => x.GetByIdAsync(negotiationId)).ReturnsAsync(negotiation);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(NegotiationStatus.Accepted, negotiation.Status);
        _negotiationRepositoryMock.Verify(x => x.UpdateAsync(negotiation), Times.Once);
    }

    [Fact]
    public async Task Handle_RejectOffer_UpdatesStatusAndTimestamp()
    {
        // Arrange
        var negotiationId = Guid.NewGuid();
        var currentTime = DateTime.UtcNow;
        var command = new ProcessOfferCommand(negotiationId, EmployeeAction.Reject);
        var negotiation = new Negotiation { Id = negotiationId, Status = NegotiationStatus.WaitingForEmployee };

        _negotiationRepositoryMock.Setup(x => x.GetByIdAsync(negotiationId)).ReturnsAsync(negotiation);
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(currentTime);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(NegotiationStatus.Rejected, negotiation.Status);
        Assert.Equal(currentTime, negotiation.LastRejectionAt);
        _negotiationRepositoryMock.Verify(x => x.UpdateAsync(negotiation), Times.Once);
    }

    [Fact]
    public async Task Handle_NegotiationNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ProcessOfferCommand(Guid.NewGuid(), EmployeeAction.Accept);
        _negotiationRepositoryMock.Setup(x => x.GetByIdAsync(command.NegotiationId)).ReturnsAsync((Negotiation)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}