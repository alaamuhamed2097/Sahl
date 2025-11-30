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
            entity.Property(e => e.TitleEn)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.TitleAr)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.CampaignType)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.StartDate)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.EndDate)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.MinimumDiscountPercentage)
                .IsRequired()
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(20m);

            entity.Property(e => e.FundingModel)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.PlatformFundingPercentage)
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.SellerFundingPercentage)
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.BannerImagePath)
                .HasMaxLength(500);

            entity.Property(e => e.ThumbnailImagePath)
                .HasMaxLength(500);

            entity.Property(e => e.ThemeColor)
                .HasMaxLength(7);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.IsFeatured)
                .HasDefaultValue(false);

            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0);

            entity.Property(e => e.SlugEn)
                .HasMaxLength(100);

            entity.Property(e => e.SlugAr)
                .HasMaxLength(100);

            entity.Property(e => e.MinimumOrderValue)
                .HasColumnType("decimal(18,2)");

            // Indexes
            entity.HasIndex(e => e.CampaignType);

            entity.HasIndex(e => e.StartDate);

            entity.HasIndex(e => e.EndDate);

            entity.HasIndex(e => e.IsActive);

            entity.HasIndex(e => e.IsFeatured);

            entity.HasIndex(e => e.SlugEn)
                .IsUnique()
                .HasFilter("[SlugEn] IS NOT NULL");

            entity.HasIndex(e => new { e.StartDate, e.EndDate, e.IsActive });
        }
    }
}
