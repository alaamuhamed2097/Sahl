using Domains.Entities.Campaign;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbCampaignProduct
    /// </summary>
    public class CampaignProductConfiguration : IEntityTypeConfiguration<TbCampaignItem>
    {
        public void Configure(EntityTypeBuilder<TbCampaignItem> entity)
        {
            // Table name
            entity.ToTable("TbCampaignProducts");

            entity.Property(e => e.CampaignPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0);

            // Relationships
            entity.HasOne(e => e.Campaign)
                .WithMany(c => c.CampaignItems)
                .HasForeignKey(e => e.CampaignId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.CampaignId);

            entity.HasIndex(e => e.ItemId);

            entity.HasIndex(e => e.IsActive);

            entity.HasIndex(e => e.DisplayOrder);

            entity.HasIndex(e => new { e.CampaignId, e.ItemId })
                .IsUnique();

            entity.HasIndex(e => new { e.CampaignId, e.IsActive });
        }
    }
}
