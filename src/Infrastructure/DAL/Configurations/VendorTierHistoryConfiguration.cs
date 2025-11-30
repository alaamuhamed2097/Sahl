using Domains.Entities.SellerTier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbVendorTierHistory
    /// </summary>
    public class VendorTierHistoryConfiguration : IEntityTypeConfiguration<TbVendorTierHistory>
    {
        public void Configure(EntityTypeBuilder<TbVendorTierHistory> entity)
        {
            // Table name
            entity.ToTable("TbVendorTierHistories");

            // Property configurations
            entity.Property(e => e.AchievedAt)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.EndedAt)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.TotalOrdersAtTime)
                .IsRequired();

            entity.Property(e => e.TotalSalesAtTime)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.IsAutomatic)
                .HasDefaultValue(true);

            entity.Property(e => e.Notes)
                .HasMaxLength(500);

            // Relationships
            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.SellerTier)
                .WithMany(t => t.VendorTierHistories)
                .HasForeignKey(e => e.SellerTierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.VendorId);

            entity.HasIndex(e => e.SellerTierId);

            entity.HasIndex(e => e.AchievedAt);

            entity.HasIndex(e => e.EndedAt);

            entity.HasIndex(e => new { e.VendorId, e.AchievedAt });
        }
    }
}
