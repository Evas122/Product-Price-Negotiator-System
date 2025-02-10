using Moq;
using PriceNegotiator.Domain.Common.Exceptions.Base;
using PriceNegotiator.Domain.Interfaces;
using PriceNegotiator.Domain.Entities.Negotiations;
using PriceNegotiator.Domain.Enums;
using PriceNegotiator.Domain.Repositories;
using PriceNegotiator.Infrastructure.Services;

namespace PriceNegotiator.Infrastructure.UnitTests.ServicesTests;

public class NegotiationValidationServiceTests
{
    [Fact]
    public async Task ValidateNewAttempt_WhenNegotiationIsCancelled_ThrowsBadRequestException()
    {
        // Arrange
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        var mockNegotiationRepository = new Mock<INegotiationRepository>();

        var negotiation = new Negotiation { Status = NegotiationStatus.Cancelled };

        var service = new NegotiationValidationService(
            mockDateTimeProvider.Object,
            mockNegotiationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(
            () => service.ValidateNewAttemptAsync(negotiation));
    }

    [Fact]
    public async Task ValidateNewAttempt_WhenAttemptLimitReached_ThrowsBadRequestException()
    {
        // Arrange
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        var mockNegotiationRepository = new Mock<INegotiationRepository>();

        var negotiation = new Negotiation
        {
            Status = NegotiationStatus.Rejected,
            AttemptCount = 3
        };

        var service = new NegotiationValidationService(
            mockDateTimeProvider.Object,
            mockNegotiationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(
            () => service.ValidateNewAttemptAsync(negotiation));
    }

    [Fact]
    public async Task ValidateNewAttempt_WhenRejectionPeriodExpired_ThrowsBadRequestException()
    {
        // Arrange
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        var mockNegotiationRepository = new Mock<INegotiationRepository>();

        var negotiation = new Negotiation
        {
            Id = Guid.NewGuid(),
            Status = NegotiationStatus.Rejected,
            LastRejectionAt = DateTime.UtcNow.AddDays(-8),
            AttemptCount = 2
        };

        mockDateTimeProvider.Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        var service = new NegotiationValidationService(
            mockDateTimeProvider.Object,
            mockNegotiationRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(
            () => service.ValidateNewAttemptAsync(negotiation));
    }
}