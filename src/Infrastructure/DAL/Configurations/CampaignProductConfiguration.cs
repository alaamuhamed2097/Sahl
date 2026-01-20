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

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            // Relationships
            entity.HasOne(e => e.Campaign)
                .WithMany(c => c.CampaignItems)
                .HasForeignKey(e => e.CampaignId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.OfferCombinationPricing)
			    .WithMany(ocp => ocp.CampaignItems) 
			    .HasForeignKey(e => e.OfferCombinationPricingId)
				.OnDelete(DeleteBehavior.Restrict);

			entity.HasIndex(e => new { e.CampaignId, e.OfferCombinationPricingId })
	            .IsUnique();

			// Indexes
			entity.HasIndex(e => e.CampaignId);

            entity.HasIndex(e => e.OfferCombinationPricingId);

            entity.HasIndex(e => e.IsActive);

            entity.HasIndex(e => new { e.CampaignId, e.OfferCombinationPricingId })
                .IsUnique();

            entity.HasIndex(e => new { e.CampaignId, e.IsActive });
        }
    }
}
