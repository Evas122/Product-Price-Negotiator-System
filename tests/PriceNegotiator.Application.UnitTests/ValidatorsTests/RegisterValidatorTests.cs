using PriceNegotiator.Domain.Commands.Register;

namespace PriceNegotiator.Domain.UnitTests.ValidatorsTests;

public class RegisterValidatorTests
{
    private readonly RegisterValidator _validator;

    public RegisterValidatorTests()
    {
        _validator = new RegisterValidator();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("invalid-email")]
    public void Validate_InvalidEmail_HasError(string email)
    {
        // Arrange
        var command = new RegisterCommand(email, "Password123!", "John", "Doe");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "Email");
    }

    [Theory]
    [InlineData("")]
    [InlineData("weak")]
    [InlineData("NoSpecialChar1")]
    [InlineData("nodigits!")]
    public void Validate_InvalidPassword_HasError(string password)
    {
        // Arrange
        var command = new RegisterCommand("test@test.com", password, "John", "Doe");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "Password");
    }

    [Fact]
    public void Validate_ValidRegistration_PassesValidation()
    {
        // Arrange
        var command = new RegisterCommand("test@test.com", "Password123!", "John", "Doe");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }
}