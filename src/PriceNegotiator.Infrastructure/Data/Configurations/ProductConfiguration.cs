using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceNegotiator.Domain.Entities.Assortments;

namespace PriceNegotiator.Infrastructure.Data.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(p => p.BasePrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.HasMany(p => p.Negotiations)
            .WithOne(n => n.Product)
            .HasForeignKey(n => n.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}