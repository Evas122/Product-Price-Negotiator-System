using Microsoft.AspNetCore.Identity;
using Moq;
using PriceNegotiator.Domain.Entities.Auth;
using PriceNegotiator.Domain.Repositories;
using PriceNegotiator.Infrastructure.Services;

namespace PriceNegotiator.Infrastructure.UnitTests.ServicesTests;

public class IdentityServiceTests
{
    [Fact]
    public async Task IsEmailUnique_WhenEmailDoesNotExist_ReturnsTrue()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var mockPasswordHasher = new Mock<IPasswordHasher<User>>();
        mockUserRepository.Setup(x => x.ExistsAsync("test@test.com"))
            .ReturnsAsync(false);

        var identityService = new IdentityService(mockUserRepository.Object, mockPasswordHasher.Object);

        // Act
        var result = await identityService.IsEmailUniqueAsync("test@test.com");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task VerifyPassword_WithCorrectPassword_ReturnsTrue()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var mockPasswordHasher = new Mock<IPasswordHasher<User>>();
        var user = new User { Email = "test@test.com" };
        var password = "password123";
        var hashedPassword = "hashedPassword123";

        mockPasswordHasher
            .Setup(x => x.VerifyHashedPassword(user, hashedPassword, password))
            .Returns(PasswordVerificationResult.Success);

        var identityService = new IdentityService(mockUserRepository.Object, mockPasswordHasher.Object);

        // Act
        var result = await identityService.VerifyPassword(user, password, hashedPassword);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task HashPassword_ReturnsHashedPassword()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var mockPasswordHasher = new Mock<IPasswordHasher<User>>();
        var user = new User { Email = "test@test.com" };
        var password = "password123";
        var expectedHash = "hashedPassword123";

        mockPasswordHasher
            .Setup(x => x.HashPassword(user, password))
            .Returns(expectedHash);

        var identityService = new IdentityService(mockUserRepository.Object, mockPasswordHasher.Object);

        // Act
        var result = await identityService.HashPassword(user, password);

        // Assert
        Assert.Equal(expectedHash, result);
    }
}