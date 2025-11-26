using Domains.Entities.Pricing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbCustomerSegmentPricing
    /// </summary>
    public class CustomerSegmentPricingConfiguration : IEntityTypeConfiguration<TbCustomerSegmentPricing>
    {
        public void Configure(EntityTypeBuilder<TbCustomerSegmentPricing> entity)
        {
            // Table name
            entity.ToTable("TbCustomerSegmentPricings");

            // Property configurations
            entity.Property(e => e.SegmentType)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.DiscountPercentage)
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.MinimumOrderQuantity)
                .IsRequired()
                .HasDefaultValue(1);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.EffectiveFrom)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.EffectiveTo)
                .HasColumnType("datetime2(2)");

            // Relationships
            entity.HasOne(e => e.Offer)
                .WithMany()
                .HasForeignKey(e => e.OfferId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.OfferId);

            entity.HasIndex(e => e.SegmentType);

            entity.HasIndex(e => e.IsActive);

            entity.HasIndex(e => new { e.OfferId, e.SegmentType })
                .IsUnique();

            entity.HasIndex(e => new { e.EffectiveFrom, e.EffectiveTo });
        }
    }
}
