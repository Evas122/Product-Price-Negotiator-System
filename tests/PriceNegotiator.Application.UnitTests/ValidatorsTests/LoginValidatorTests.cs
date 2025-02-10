using PriceNegotiator.Domain.Commands.Login;

namespace PriceNegotiator.Domain.UnitTests.ValidatorsTests;

public class LoginValidatorTests
{
    private readonly LoginValidator _validator;

    public LoginValidatorTests()
    {
        _validator = new LoginValidator();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("invalid-email")]
    public void Validate_InvalidEmail_HasError(string email)
    {
        // Arrange
        var command = new LoginCommand(email, "password");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "Email");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_EmptyPassword_HasError(string password)
    {
        // Arrange
        var command = new LoginCommand("test@test.com", password);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "Password");
    }

    [Fact]
    public void Validate_ValidCredentials_PassesValidation()
    {
        // Arrange
        var command = new LoginCommand("test@test.com", "password123");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }
}