using Microsoft.EntityFrameworkCore;
using PriceNegotiator.Domain.Entities.Auth;
using PriceNegotiator.Infrastructure.Data;
using PriceNegotiator.Infrastructure.Repositories;

namespace PriceNegotiator.Infrastructure.UnitTests.RepositoriesTests;

public class UserRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldAddUserToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        using var context = new AppDbContext(options);
        var repository = new UserRepository(context);
        var user = new User { Id = Guid.NewGuid(), Email = "test@test.com", FirstName = "test", LastName = "testtest", PasswordHash = "HashedPassword" };

        // Act
        await repository.AddAsync(user);

        // Assert
        var savedUser = await context.Users.FindAsync(user.Id);
        Assert.NotNull(savedUser);
        Assert.Equal(user.Email, savedUser.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_ReturnsCorrectUser()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        using var context = new AppDbContext(options);
        var repository = new UserRepository(context);
        var user = new User { Id = Guid.NewGuid(), Email = "test@test.com", FirstName = "test", LastName = "testtest", PasswordHash = "HashedPassword" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByEmailAsync("test@test.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsTrueForExistingEmail()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        using var context = new AppDbContext(options);
        var repository = new UserRepository(context);
        var user = new User { Id = Guid.NewGuid(), Email = "test@test.com", FirstName = "test", LastName = "testtest", PasswordHash = "HashedPassword" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var exists = await repository.ExistsAsync("test@test.com");

        // Assert
        Assert.True(exists);
    }
}