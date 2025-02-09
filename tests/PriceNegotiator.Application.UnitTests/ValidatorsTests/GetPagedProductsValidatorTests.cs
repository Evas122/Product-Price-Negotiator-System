using PriceNegotiator.Application.Queries.GetPagedProducts;

namespace PriceNegotiator.Application.UnitTests.ValidatorsTests;

public class GetPagedProductsValidatorTests
{
    private readonly GetPagedProductsValidator _validator;

    public GetPagedProductsValidatorTests()
    {
        _validator = new GetPagedProductsValidator();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_InvalidPage_HasError(int page)
    {
        // Arrange
        var query = new GetPagedProductsQuery(page, 10);

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "Page");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(11)]
    public void Validate_InvalidPageSize_HasError(int pageSize)
    {
        // Arrange
        var query = new GetPagedProductsQuery(1, pageSize);

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "PageSize");
    }

    [Fact]
    public void Validate_ValidQuery_PassesValidation()
    {
        // Arrange
        var query = new GetPagedProductsQuery(1, 10);

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.True(result.IsValid);
    }
}