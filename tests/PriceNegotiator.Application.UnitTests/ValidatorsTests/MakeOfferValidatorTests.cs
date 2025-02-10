using FluentValidation.TestHelper;
using PriceNegotiator.Domain.Commands.MakeOffer;

namespace PriceNegotiator.Domain.UnitTests.ValidatorsTests;

public class MakeOfferValidatorTests
{
    private readonly MakeOfferValidator _validator;

    public MakeOfferValidatorTests()
    {
        _validator = new MakeOfferValidator();
    }

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new MakeOfferCommand(
            ClientEmail: "test@test.com",
            ProductId: Guid.NewGuid(),
            ProposedPrice: 100m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-email")]
    [InlineData(null)]
    public void Validate_WithInvalidEmail_ShouldHaveValidationError(string email)
    {
        // Arrange
        var command = new MakeOfferCommand(
            ClientEmail: email,
            ProductId: Guid.NewGuid(),
            ProposedPrice: 100m);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ClientEmail);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithInvalidPrice_ShouldHaveValidationError(decimal price)
    {
        // Arrange
        var command = new MakeOfferCommand(
            ClientEmail: "test@test.com",
            ProductId: Guid.NewGuid(),
            ProposedPrice: price);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProposedPrice);
    }
}