using Domains.Entities.Offer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// TbOfferCombinationPricing configuration optimized for price/stock queries
    /// Indexes designed for high-frequency read operations
    /// </summary>
    public class OfferPricingSearchConfiguration : IEntityTypeConfiguration<TbOfferCombinationPricing>
    {
        public void Configure(EntityTypeBuilder<TbOfferCombinationPricing> builder)
        {
            builder.HasKey(e => e.Id);
            builder.ToTable("TbOfferCombinationPricings");

            // ============================================================================
            // Column Configurations
            // ============================================================================

            builder.Property(e => e.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(e => e.SalesPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(e => e.AvailableQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.StockStatus).IsRequired();

            // ============================================================================
            // STRATEGIC INDEXES FOR PRICE AND STOCK QUERIES
            // ============================================================================

            // ✅ 1. COMPOSITE COVERING INDEX for Offer → Pricing lookup
            // Usage: JOIN TbOfferCombinationPricing ON OfferId WHERE IsDeleted = 0
            // This is CRITICAL for search - retrieves all pricing data in one seek
            builder.HasIndex(e => new { e.OfferId, e.IsDeleted })
                .HasDatabaseName("IX_TbOfferPricing_OfferId_Deleted_Covering_NC")
                .IsUnique(false)
                .IncludeProperties(e => new
                {
                    e.SalesPrice,
                    e.Price,
                    e.AvailableQuantity,
                    e.StockStatus
                });

            // ✅ 2. NONCLUSTERED INDEX for Price Sorting
            // Usage: ORDER BY SalesPrice ASC/DESC
            builder.HasIndex(e => e.SalesPrice)
                .HasDatabaseName("IX_TbOfferPricing_SalesPrice_NC")
                .IsUnique(false);

            // ✅ 3. COMPOSITE INDEX for Price Range Filtering
            // Usage: WHERE SalesPrice BETWEEN @MinPrice AND @MaxPrice
            builder.HasIndex(e => new { e.SalesPrice, e.IsDeleted })
                .HasDatabaseName("IX_TbOfferPricing_SalesPrice_Deleted_NC")
                .IsUnique(false)
                .HasFilter("[IsDeleted] = 0");

            // ✅ 4. FILTERED INDEX for In-Stock Items Only
            // Usage: WHERE AvailableQuantity > 0 AND StockStatus = 1
            // Dramatically reduces index size (excludes out-of-stock)
            builder.HasIndex(e => new { e.StockStatus, e.AvailableQuantity })
                .HasDatabaseName("IX_TbOfferPricing_InStock_Filtered_NC")
                .IsUnique(false)
                .HasFilter("[AvailableQuantity] > 0 AND [StockStatus] = 1 AND [IsDeleted] = 0");

            // ============================================================================
            // Foreign Key Relationships
            // ============================================================================

            builder.HasOne(e => e.Offer)
                .WithMany(o => o.OfferCombinationPricings)
                .HasForeignKey(e => e.OfferId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ItemCombination)
                .WithMany()
                .HasForeignKey(e => e.ItemCombinationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

