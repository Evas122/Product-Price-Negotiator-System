using PriceNegotiator.Application.Commands.CreateProduct;

namespace PriceNegotiator.Application.UnitTests.ValidatorsTests;

public class ValidatorTests
{
    public class CreateProductValidatorTests
    {
        private readonly CreateProductValidator _validator;

        public CreateProductValidatorTests()
        {
            _validator = new CreateProductValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_EmptyName_HasError(string name)
        {
            // Arrange
            var command = new CreateProductCommand(name, "Description", 100m);

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == "Name");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_EmptyDescription_HasError(string description)
        {
            // Arrange
            var command = new CreateProductCommand("Name", description, 100m);

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == "Description");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validate_InvalidBasePrice_HasError(decimal basePrice)
        {
            // Arrange
            var command = new CreateProductCommand("Name", "Description", basePrice);

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == "BasePrice");
        }
    }
}