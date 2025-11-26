using Domains.Entities.Campaign;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbCampaignProduct
    /// </summary>
    public class CampaignProductConfiguration : IEntityTypeConfiguration<TbCampaignProduct>
    {
        public void Configure(EntityTypeBuilder<TbCampaignProduct> entity)
        {
            // Table name
            entity.ToTable("TbCampaignProducts");

            // Property configurations
            entity.Property(e => e.OriginalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.CampaignPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.DiscountPercentage)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.PlatformContribution)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.VendorContribution)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.SoldQuantity)
                .HasDefaultValue(0);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.ApprovedAt)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0);

            // Relationships
            entity.HasOne(e => e.Campaign)
                .WithMany(c => c.CampaignProducts)
                .HasForeignKey(e => e.CampaignId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

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

            entity.HasIndex(e => e.ItemId);

            entity.HasIndex(e => e.VendorId);

            entity.HasIndex(e => e.IsActive);

            entity.HasIndex(e => e.DisplayOrder);

            entity.HasIndex(e => new { e.CampaignId, e.ItemId })
                .IsUnique();

            entity.HasIndex(e => new { e.CampaignId, e.IsActive });
        }
    }
}
