using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PriceNegotiator.Domain.Entities.Negotiations;

namespace PriceNegotiator.Infrastructure.Data.Configurations;

internal sealed class NegotiationConfiguration : IEntityTypeConfiguration<Negotiation>
{
    public void Configure(EntityTypeBuilder<Negotiation> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.ClientEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(n => n.AttemptCount)
            .IsRequired();

        builder.Property(n => n.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(n => n.LastRejectionAt)
            .IsRequired(false);

        builder.HasOne(n => n.Product)
            .WithMany(p => p.Negotiations)
            .HasForeignKey(n => n.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(n => n.NegotiationAttempts)
            .WithOne(na => na.Negotiation)
            .HasForeignKey(na => na.NegotiationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}