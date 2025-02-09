using Moq;
using PriceNegotiator.Application.Commands.Register;
using PriceNegotiator.Application.Common.Exceptions.Base;
using PriceNegotiator.Application.Interfaces;
using PriceNegotiator.Domain.Entities.Auth;
using PriceNegotiator.Domain.Enums;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Application.UnitTests.CommandsTests;

public class RegisterHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly RegisterHandler _handler;

    public RegisterHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _identityServiceMock = new Mock<IIdentityService>();
        _jwtServiceMock = new Mock<IJwtService>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _handler = new RegisterHandler(_userRepositoryMock.Object, _identityServiceMock.Object,
            _jwtServiceMock.Object, _dateTimeProviderMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRegistration_ReturnsAuthResult()
    {
        // Arrange
        var command = new RegisterCommand("test@test.com", "Password123!", "John", "Doe");
        var currentTime = DateTime.UtcNow;
        var token = "test-token";
        var hashedPassword = "hashedPassword";

        _identityServiceMock.Setup(x => x.IsEmailUniqueAsync(command.Email)).ReturnsAsync(true);
        _identityServiceMock.Setup(x => x.HashPassword(It.IsAny<User>(), command.Password)).ReturnsAsync(hashedPassword);
        _jwtServiceMock.Setup(x => x.GenerateJwtToken(It.IsAny<User>())).Returns(token);
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(currentTime);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(token, result.Token);
        _userRepositoryMock.Verify(x => x.AddAsync(It.Is<User>(u =>
            u.Email == command.Email &&
            u.FirstName == command.FirstName &&
            u.LastName == command.LastName &&
            u.PasswordHash == hashedPassword &&
            u.Role == UserRole.Employee)), Times.Once);
    }

    [Fact]
    public async Task Handle_EmailAlreadyExists_ThrowsAlreadyExistsException()
    {
        // Arrange
        var command = new RegisterCommand("test@test.com", "Password123!", "John", "Doe");
        _identityServiceMock.Setup(x => x.IsEmailUniqueAsync(command.Email)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<AlreadyExistsException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ValidRegistration_SetsCorrectTimestamps()
    {
        // Arrange
        var command = new RegisterCommand("test@test.com", "Password123!", "John", "Doe");
        var currentTime = DateTime.UtcNow;
        _identityServiceMock.Setup(x => x.IsEmailUniqueAsync(command.Email)).ReturnsAsync(true);
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(currentTime);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(x => x.AddAsync(It.Is<User>(u =>
            u.CreatedAt == currentTime &&
            u.UpdatedAt == currentTime)), Times.Once);
    }
}