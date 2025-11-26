using Domains.Entities.Pricing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbQuantityPricing
    /// </summary>
    public class QuantityPricingConfiguration : IEntityTypeConfiguration<TbQuantityPricing>
    {
        public void Configure(EntityTypeBuilder<TbQuantityPricing> entity)
        {
            // Table name
            entity.ToTable("TbQuantityPricings");

            // Property configurations
            entity.Property(e => e.MinimumQuantity)
                .IsRequired();

            entity.Property(e => e.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.DiscountPercentage)
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0);

            // Relationships
            entity.HasOne(e => e.Offer)
                .WithMany()
                .HasForeignKey(e => e.OfferId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.OfferId);

            entity.HasIndex(e => e.MinimumQuantity);

            entity.HasIndex(e => e.IsActive);

            entity.HasIndex(e => new { e.OfferId, e.MinimumQuantity })
                .IsUnique();

            // Check constraint
            entity.HasCheckConstraint("CK_QuantityPricing_Quantities", "[MaximumQuantity] IS NULL OR [MaximumQuantity] > [MinimumQuantity]");
        }
    }
}
