namespace PriceNegotiator.Application.Dtos.Assortment;

public record ProductDto(Guid Id, string Name, string Description, decimal BasePrice);