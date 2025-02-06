using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceNegotiator.Domain.Entities.Assortment;

namespace PriceNegotiator.Infrastructure.Data.Configurations.Assortment;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(p => p.BasePrice)
            .HasColumnType("decimal(18,2)");
    }
}