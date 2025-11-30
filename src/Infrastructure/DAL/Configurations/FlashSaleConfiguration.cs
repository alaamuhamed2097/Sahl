using Domains.Entities.Campaign;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbFlashSale
    /// </summary>
    public class FlashSaleConfiguration : IEntityTypeConfiguration<TbFlashSale>
    {
        public void Configure(EntityTypeBuilder<TbFlashSale> entity)
        {
            // Table name
            entity.ToTable("TbFlashSales");

            // Property configurations
            entity.Property(e => e.TitleEn)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.TitleAr)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.StartDate)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.EndDate)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.DurationInHours)
                .IsRequired();

            entity.Property(e => e.MinimumDiscountPercentage)
                .IsRequired()
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(20m);

            entity.Property(e => e.MinimumSellerRating)
                .HasColumnType("decimal(3,2)")
                .HasDefaultValue(4.0m);

            entity.Property(e => e.BannerImagePath)
                .HasMaxLength(500);

            entity.Property(e => e.ThemeColor)
                .HasMaxLength(7);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.ShowCountdownTimer)
                .HasDefaultValue(true);

            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0);

            // Check constraint for DurationInHours
            entity.HasCheckConstraint("CK_FlashSale_DurationInHours", "[DurationInHours] >= 6 AND [DurationInHours] <= 48");

            // Indexes
            entity.HasIndex(e => e.StartDate);

            entity.HasIndex(e => e.EndDate);

            entity.HasIndex(e => e.IsActive);

            entity.HasIndex(e => new { e.StartDate, e.EndDate, e.IsActive });
        }
    }
}
