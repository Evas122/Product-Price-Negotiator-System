using PriceNegotiator.Domain.Common;

namespace PriceNegotiator.Domain.Entities.Assortment;

public class Product : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal BasePrice { get; set; }
}