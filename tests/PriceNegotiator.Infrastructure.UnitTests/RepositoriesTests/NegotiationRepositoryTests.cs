using Microsoft.EntityFrameworkCore;
using PriceNegotiator.Domain.Entities.Negotiations;
using PriceNegotiator.Domain.Enums;
using PriceNegotiator.Infrastructure.Data;
using PriceNegotiator.Infrastructure.Repositories;

namespace PriceNegotiator.Infrastructure.UnitTests.RepositoriesTests;

public class NegotiationRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldAddNegotiationToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        using var context = new AppDbContext(options);
        var repository = new NegotiationRepository(context);
        var negotiation = new Negotiation
        {
            Id = Guid.NewGuid(),
            ClientEmail = "test@test.com",
            ProductId = Guid.NewGuid()
        };

        // Act
        await repository.AddAsync(negotiation);

        // Assert
        var savedNegotiation = await context.Negotiations.FindAsync(negotiation.Id);
        Assert.NotNull(savedNegotiation);
        Assert.Equal(negotiation.ClientEmail, savedNegotiation.ClientEmail);
    }

    [Fact]
    public async Task UpdateStatusAsync_UpdatesNegotiationStatus()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
           .Options;

        using var context = new AppDbContext(options);
        var repository = new NegotiationRepository(context);
        var negotiation = new Negotiation
        {
            Id = Guid.NewGuid(),
            ClientEmail = "test@test.com",
            ProductId = Guid.NewGuid(),
            Status = NegotiationStatus.WaitingForEmployee
        };
        context.Negotiations.Add(negotiation);
        await context.SaveChangesAsync();

        // Act
        await repository.UpdateStatusAsync(negotiation.Id, NegotiationStatus.Accepted);

        // Assert
        var updatedNegotiation = await context.Negotiations.FindAsync(negotiation.Id);
        Assert.NotNull(updatedNegotiation);
        Assert.Equal(NegotiationStatus.Accepted, updatedNegotiation.Status);
    }

    [Fact]
    public async Task GetByClientEmailandProductIdAsync_ReturnsCorrectNegotiation()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        using var context = new AppDbContext(options);
        var repository = new NegotiationRepository(context);
        var productId = Guid.NewGuid();
        var negotiation = new Negotiation
        {
            Id = Guid.NewGuid(),
            ClientEmail = "test@test.com",
            ProductId = productId,
            Status = NegotiationStatus.WaitingForEmployee
        };
        context.Negotiations.Add(negotiation);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByClientEmailandProductIdAsync("test@test.com", productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(negotiation.Id, result.Id);
        Assert.Equal(negotiation.ClientEmail, result.ClientEmail);
        Assert.Equal(negotiation.ProductId, result.ProductId);
    }
}