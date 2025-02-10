using Microsoft.Extensions.Options;
using Moq;
using PriceNegotiator.Domain.Common.Options;
using PriceNegotiator.Domain.Interfaces;
using PriceNegotiator.Domain.Entities.Auth;
using PriceNegotiator.Domain.Enums;
using PriceNegotiator.Infrastructure.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PriceNegotiator.Infrastructure.UnitTests.ServicesTests;

public class JwtServiceTests
{
    [Fact]
    public void GenerateJwtToken_ValidUser_ReturnsValidToken()
    {
        // Arrange
        var jwtSettings = new JwtSettings
        {
            SecretKey = "your-256-bit-secret-key-here-for-testing",
            Issuer = "test-issuer",
            Audience = "test-audience",
            AccessTokenExpirationMinutes = 60
        };
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        var currentTime = DateTime.UtcNow;
        mockDateTimeProvider.Setup(x => x.UtcNow).Returns(currentTime);

        var jwtService = new JwtService(
            Options.Create(jwtSettings),
            mockDateTimeProvider.Object);

        var user = new User { Id = Guid.NewGuid(), Email = "test@test.com", Role = UserRole.Employee };

        // Act
        var token = jwtService.GenerateJwtToken(user);

        // Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        Assert.Equal(user.Email, jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value);
        Assert.Equal(user.Id.ToString(), jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
    }

    [Fact]
    public void GenerateJwtToken_ValidUser_ContainsRoleClaim()
    {
        // Arrange
        var jwtSettings = new JwtSettings
        {
            SecretKey = "your-256-bit-secret-key-here-for-testing",
            Issuer = "test-issuer",
            Audience = "test-audience",
            AccessTokenExpirationMinutes = 60
        };
        var mockDateTimeProvider = new Mock<IDateTimeProvider>();
        mockDateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        var jwtService = new JwtService(
            Options.Create(jwtSettings),
            mockDateTimeProvider.Object);

        var user = new User { Id = Guid.NewGuid(), Email = "test@test.com", Role = UserRole.Employee };

        // Act
        var token = jwtService.GenerateJwtToken(user);

        // Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        Assert.Equal(UserRole.Employee.ToString(),
            jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value);
    }
}