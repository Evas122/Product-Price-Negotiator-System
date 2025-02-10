namespace PriceNegotiator.Domain.Dtos.Assortment;

public record ProductDto(Guid Id, string Name, string Description, decimal BasePrice);