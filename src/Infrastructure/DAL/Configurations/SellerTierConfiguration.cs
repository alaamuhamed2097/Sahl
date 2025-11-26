using Domains.Entities.SellerTier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbSellerTier
    /// </summary>
    public class SellerTierConfiguration : IEntityTypeConfiguration<TbSellerTier>
    {
        public void Configure(EntityTypeBuilder<TbSellerTier> entity)
        {
            // Table name
            entity.ToTable("TbSellerTiers");

            // Property configurations
            entity.Property(e => e.TierCode)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.TierNameEn)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.TierNameAr)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(500);

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(500);

            entity.Property(e => e.MinimumOrders)
                .IsRequired();

            entity.Property(e => e.CommissionReductionPercentage)
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.HasPrioritySupport)
                .HasDefaultValue(false);

            entity.Property(e => e.HasBuyBoxBoost)
                .HasDefaultValue(false);

            entity.Property(e => e.HasFeaturedListings)
                .HasDefaultValue(false);

            entity.Property(e => e.HasAdvancedAnalytics)
                .HasDefaultValue(false);

            entity.Property(e => e.HasDedicatedAccountManager)
                .HasDefaultValue(false);

            entity.Property(e => e.BadgeColor)
                .HasMaxLength(50);

            entity.Property(e => e.BadgeIconPath)
                .HasMaxLength(200);

            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            // Indexes
            entity.HasIndex(e => e.TierCode)
                .IsUnique();

            entity.HasIndex(e => e.DisplayOrder);

            entity.HasIndex(e => e.IsActive);

            entity.HasIndex(e => new { e.MinimumOrders, e.MaximumOrders });

            // Check constraint
            entity.HasCheckConstraint("CK_SellerTier_Orders", "[MaximumOrders] IS NULL OR [MaximumOrders] > [MinimumOrders]");
        }
    }
}
