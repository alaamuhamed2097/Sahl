using Domains.Entities.Loyalty;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbCustomerLoyalty
    /// </summary>
    public class CustomerLoyaltyConfiguration : IEntityTypeConfiguration<TbCustomerLoyalty>
    {
        public void Configure(EntityTypeBuilder<TbCustomerLoyalty> entity)
        {
            // Table name
            entity.ToTable("TbCustomerLoyalties");

            // Property configurations
            entity.Property(e => e.TotalPoints)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.AvailablePoints)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.UsedPoints)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.ExpiredPoints)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.TotalOrdersThisYear)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(e => e.TotalSpentThisYear)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.LastTierUpgradeDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.NextTierEligibilityDate)
                .HasColumnType("datetime2(2)");

            // Relationships
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.LoyaltyTier)
                .WithMany(t => t.CustomerLoyalties)
                .HasForeignKey(e => e.LoyaltyTierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.UserId)
                .IsUnique();

            entity.HasIndex(e => e.LoyaltyTierId);

            entity.HasIndex(e => e.AvailablePoints);
        }
    }
}
