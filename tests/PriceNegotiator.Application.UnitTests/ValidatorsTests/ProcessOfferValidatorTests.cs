using PriceNegotiator.Domain.Commands.ProcessOffer;
using PriceNegotiator.Domain.Enums;

namespace PriceNegotiator.Domain.UnitTests.ValidatorsTests;

public class ProcessOfferValidatorTests
{
    private readonly ProcessOfferValidator _validator;

    public ProcessOfferValidatorTests()
    {
        _validator = new ProcessOfferValidator();
    }

    [Fact]
    public void Validate_EmptyNegotiationId_HasError()
    {
        // Arrange
        var command = new ProcessOfferCommand(Guid.Empty, EmployeeAction.Accept);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "NegotiationId");
    }

    [Fact]
    public void Validate_InvalidAction_HasError()
    {
        // Arrange
        var command = new ProcessOfferCommand(Guid.NewGuid(), (EmployeeAction)999);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "Action");
    }

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        // Arrange
        var command = new ProcessOfferCommand(Guid.NewGuid(), EmployeeAction.Accept);
        // Act
        var result = _validator.Validate(command);
        // Assert
        Assert.True(result.IsValid);
    }
}