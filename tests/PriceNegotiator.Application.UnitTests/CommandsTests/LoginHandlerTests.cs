using Moq;
using PriceNegotiator.Application.Commands.Login;
using PriceNegotiator.Application.Common.Exceptions.Base;
using PriceNegotiator.Application.Interfaces;
using PriceNegotiator.Domain.Entities.Auth;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Application.UnitTests.CommandsTests;

public class LoginHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly LoginHandler _handler;

    public LoginHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _identityServiceMock = new Mock<IIdentityService>();
        _jwtServiceMock = new Mock<IJwtService>();
        _handler = new LoginHandler(_userRepositoryMock.Object, _identityServiceMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsCorrectToken()
    {
        // Arrange
        var command = new LoginCommand("test@test.com", "password");
        var user = new User { Email = command.Email, PasswordHash = "hashedPassword" };
        var token = "test-token";

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email)).ReturnsAsync(user);
        _identityServiceMock.Setup(x => x.VerifyPassword(user, command.Password, user.PasswordHash)).ReturnsAsync(true);
        _jwtServiceMock.Setup(x => x.GenerateJwtToken(user)).Returns(token);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(token, result.Token);
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new LoginCommand("test@test.com", "password");
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email)).ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidPassword_ThrowsInvalidCredentialsException()
    {
        // Arrange
        var command = new LoginCommand("test@test.com", "password");
        var user = new User { Email = command.Email, PasswordHash = "hashedPassword" };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email)).ReturnsAsync(user);
        _identityServiceMock.Setup(x => x.VerifyPassword(user, command.Password, user.PasswordHash)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}