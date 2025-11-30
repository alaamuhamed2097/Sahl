using Domains.Entities.Campaign;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbCampaignVendor
    /// </summary>
    public class CampaignVendorConfiguration : IEntityTypeConfiguration<TbCampaignVendor>
    {
        public void Configure(EntityTypeBuilder<TbCampaignVendor> entity)
        {
            // Table name
            entity.ToTable("TbCampaignVendors");

            // Property configurations
            entity.Property(e => e.IsApproved)
                .HasDefaultValue(false);

            entity.Property(e => e.AppliedAt)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.ApprovedAt)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.TotalProductsSubmitted)
                .HasDefaultValue(0);

            entity.Property(e => e.TotalProductsApproved)
                .HasDefaultValue(0);

            entity.Property(e => e.TotalSales)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.TotalCommissionPaid)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.Notes)
                .HasMaxLength(1000);

            // Relationships
            entity.HasOne(e => e.Campaign)
                .WithMany(c => c.CampaignVendors)
                .HasForeignKey(e => e.CampaignId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ApprovedByUser)
                .WithMany()
                .HasForeignKey(e => e.ApprovedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.CampaignId);

            entity.HasIndex(e => e.VendorId);

            entity.HasIndex(e => e.IsApproved);

            entity.HasIndex(e => new { e.CampaignId, e.VendorId })
                .IsUnique();
        }
    }
}
