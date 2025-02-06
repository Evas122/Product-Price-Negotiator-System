using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PriceNegotiator.Domain.Entities.Negotiations;

namespace PriceNegotiator.Infrastructure.Data.Configurations;

internal sealed class NegotiationAttemptConfiguration : IEntityTypeConfiguration<NegotiationAttempt>
{
    public void Configure(EntityTypeBuilder<NegotiationAttempt> builder)
    {
        builder.HasKey(na => na.Id);

        builder.Property(na => na.ProposedPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(na => na.ProposedAt)
            .IsRequired();

        builder.Property(na => na.AttemptNumber)
            .IsRequired();

        builder.HasOne(na => na.Negotiation)
            .WithMany(n => n.NegotiationAttempts)
            .HasForeignKey(na => na.NegotiationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}