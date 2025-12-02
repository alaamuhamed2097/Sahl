using Domains.Entities.Offer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Extensions
{
    public class OfferPriceHistoryConfiguration : IEntityTypeConfiguration<TbOfferPriceHistory>
{
    public void Configure(EntityTypeBuilder<TbOfferPriceHistory> builder)
    {
        builder.ToTable("TbOfferPriceHistories");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.OldPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.NewPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.ChangeNote)
            .HasMaxLength(500);

        builder.Property(e => e.CreatedDateUtc)
            .HasColumnType("datetime2(2)");

        // Relationships
        builder.HasOne(e => e.ItemCombination)
               .WithMany(ic => ic.OfferPriceHistories) // add collection on ItemCombination if needed, or change WithMany()
               .HasForeignKey(e => e.ItemCombinationId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.OfferCombinationPricing)
               .WithMany(op => op.OfferPriceHistories) // add collection on OfferCombinationPricing if needed
               .HasForeignKey(e => e.OfferCombinationPricingId)
               .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(e => e.ItemCombinationId);
        builder.HasIndex(e => e.OfferCombinationPricingId);
    }
}
}
