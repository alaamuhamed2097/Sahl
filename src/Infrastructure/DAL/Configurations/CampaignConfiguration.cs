using Domains.Entities.Campaign;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbCampaign
    /// </summary>
    public class CampaignConfiguration : IEntityTypeConfiguration<TbCampaign>
    {
        public void Configure(EntityTypeBuilder<TbCampaign> entity)
        {
            // Table name
            entity.ToTable("TbCampaigns");

            // Property configurations
            entity.Property(e => e.StartDate)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.EndDate)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            // Indexes
            entity.HasIndex(e => e.StartDate);

            entity.HasIndex(e => e.EndDate);

            entity.HasIndex(e => e.IsActive);

            entity.HasIndex(e => new { e.StartDate, e.EndDate, e.IsActive });
        }
    }
}
