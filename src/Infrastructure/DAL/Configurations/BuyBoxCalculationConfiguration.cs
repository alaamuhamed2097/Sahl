using Domains.Entities.BuyBox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbBuyBoxCalculation
    /// </summary>
    public class BuyBoxCalculationConfiguration : IEntityTypeConfiguration<TbBuyBoxCalculation>
    {
        public void Configure(EntityTypeBuilder<TbBuyBoxCalculation> entity)
        {
            // Table name
            entity.ToTable("TbBuyBoxCalculations");

            // Property configurations
            entity.Property(e => e.PriceScore)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.SellerRatingScore)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.ShippingSpeedScore)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.FBMUsageScore)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.StockLevelScore)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.ReturnRateScore)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.TotalScore)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.CalculatedAt)
                .IsRequired()
                .HasColumnType("datetime2(2)")
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.CalculationDetails)
                .HasMaxLength(1000);

            // Relationships
            entity.HasOne(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.WinningOffer)
                .WithMany()
                .HasForeignKey(e => e.WinningOfferId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.ItemId);

            entity.HasIndex(e => e.WinningOfferId);

            entity.HasIndex(e => e.TotalScore);

            entity.HasIndex(e => e.CalculatedAt);

            entity.HasIndex(e => e.ExpiresAt);

            entity.HasIndex(e => new { e.ItemId, e.CalculatedAt });
        }
    }
}
