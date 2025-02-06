using PriceNegotiator.Domain.Common;
using PriceNegotiator.Domain.Entities.Negotiations;

namespace PriceNegotiator.Domain.Entities.Assortments;

public class Product : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal BasePrice { get; set; }
    public ICollection<Negotiation> Negotiations { get; set; } = [];
}