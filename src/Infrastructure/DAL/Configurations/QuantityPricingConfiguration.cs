using Domains.Entities.Catalog.Pricing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbQuantityPricing
    /// </summary>
    public class QuantityPricingConfiguration : IEntityTypeConfiguration<TbQuantityTierPricing>
    {
        public void Configure(EntityTypeBuilder<TbQuantityTierPricing> entity)
        {
            // Table name
            entity.ToTable("TbQuantityTierPricings");

            // Property configurations
            entity.Property(e => e.MinQuantity)
                .IsRequired();

            entity.Property(e => e.PricePerUnit)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.SalesPricePerUnit)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.DiscountPercentage)
                .HasColumnType("decimal(5,2)");

            // Relationships
            entity.HasOne(e => e.OfferCombinationPricing)
                .WithMany(oc=>oc.QuantityTierPricings)
                .HasForeignKey(e => e.OfferCombinationPricingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.OfferCombinationPricingId);

            entity.HasIndex(e => e.MinQuantity);
            entity.HasIndex(e => e.MaxQuantity);

            entity.HasIndex(e => new { e.OfferCombinationPricingId, e.MinQuantity, e.MaxQuantity})
                .IsUnique(true);

            // Check constraint
            entity.HasCheckConstraint("CK_QuantityPricing_Quantities", "[MaxQuantity] IS NULL OR [MaxQuantity] > [MinQuantity]");
        }
    }
}
