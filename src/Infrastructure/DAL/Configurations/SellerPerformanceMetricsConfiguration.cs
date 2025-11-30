using Domains.Entities.BuyBox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbSellerPerformanceMetrics
    /// </summary>
    public class SellerPerformanceMetricsConfiguration : IEntityTypeConfiguration<TbSellerPerformanceMetrics>
    {
        public void Configure(EntityTypeBuilder<TbSellerPerformanceMetrics> entity)
        {
            // Table name
            entity.ToTable("TbSellerPerformanceMetrics");

            // Property configurations
            entity.Property(e => e.AverageRating)
                .IsRequired()
                .HasColumnType("decimal(3,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.TotalReviews)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(e => e.OrderCompletionRate)
                .IsRequired()
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.OnTimeShippingRate)
                .IsRequired()
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.ReturnRate)
                .IsRequired()
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.CancellationRate)
                .IsRequired()
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.ResponseRate)
                .IsRequired()
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.AverageResponseTimeInHours)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(e => e.BuyBoxWins)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(e => e.BuyBoxWinRate)
                .IsRequired()
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.UsesFBM)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(e => e.LastUpdated)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.CalculatedForPeriodStart)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.CalculatedForPeriodEnd)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            // Relationships
            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.VendorId);

            entity.HasIndex(e => e.AverageRating);

            entity.HasIndex(e => e.OrderCompletionRate);

            entity.HasIndex(e => e.BuyBoxWinRate);

            entity.HasIndex(e => e.UsesFBM);

            entity.HasIndex(e => new { e.VendorId, e.CalculatedForPeriodStart });
        }
    }
}
