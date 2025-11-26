using Domains.Entities.Loyalty;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbLoyaltyTier
    /// </summary>
    public class LoyaltyTierConfiguration : IEntityTypeConfiguration<TbLoyaltyTier>
    {
        public void Configure(EntityTypeBuilder<TbLoyaltyTier> entity)
        {
            // Table name
            entity.ToTable("TbLoyaltyTiers");

            // Property configurations
            entity.Property(e => e.TierNameEn)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.TierNameAr)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.MinimumOrdersPerYear)
                .IsRequired();

            entity.Property(e => e.MaximumOrdersPerYear)
                .IsRequired();

            entity.Property(e => e.PointsMultiplier)
                .IsRequired()
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(1.0m);

            entity.Property(e => e.CashbackPercentage)
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.HasFreeShipping)
                .HasDefaultValue(false);

            entity.Property(e => e.HasPrioritySupport)
                .HasDefaultValue(false);

            entity.Property(e => e.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(500);

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(500);

            entity.Property(e => e.BadgeColor)
                .HasMaxLength(50);

            entity.Property(e => e.BadgeIconPath)
                .HasMaxLength(200);

            // Indexes
            entity.HasIndex(e => e.TierNameEn)
                .IsUnique();

            entity.HasIndex(e => e.DisplayOrder);

            entity.HasIndex(e => new { e.MinimumOrdersPerYear, e.MaximumOrdersPerYear });
        }
    }
}
