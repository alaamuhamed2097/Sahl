using Common.Enumerations.Fulfillment;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Offer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations.Offer
{
    /// <summary>
    /// Fluent API configuration for TbOfferCombinationPricing entity
    /// This configuration includes critical performance indexes for search operations
    /// NOTE: This configuration is supplementary. The migration 
    /// (OptimizeItemSearchPerformance) already creates these indexes in the database.
    /// </summary>
    public class OfferCombinationPricingConfiguration : IEntityTypeConfiguration<TbOfferCombinationPricing>
    {
        public void Configure(EntityTypeBuilder<TbOfferCombinationPricing> builder)
        {
            // Primary key
            builder.HasKey(e => e.Id);

            // Table name
            builder.ToTable("TbOfferCombinationPricing");

            // ============================================================================
            // Column Configurations
            // ============================================================================

            // Price columns (decimal with precision)
            builder.Property(e => e.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(e => e.SalesPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(e => e.CostPrice)
                .HasColumnType("decimal(18,2)");

            // Stock Management
            builder.Property(e => e.AvailableQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.ReservedQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.RefundedQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.DamagedQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.InTransitQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.ReturnedQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.LockedQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            // Stock thresholds
            builder.Property(e => e.MinOrderQuantity)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(e => e.MaxOrderQuantity)
                .IsRequired()
                .HasDefaultValue(999);

            builder.Property(e => e.LowStockThreshold)
                .IsRequired()
                .HasDefaultValue(5);

            // Status and flags
            builder.Property(e => e.IsDefault)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(e => e.StockStatus)
                .IsRequired();

            // Timestamps
            builder.Property(e => e.LastPriceUpdate)
                .HasColumnType("datetime2(2)");

            builder.Property(e => e.LastStockUpdate)
                .HasColumnType("datetime2(2)");

            // ============================================================================
            // Foreign key relationships (if not already configured in another class)
            // ============================================================================

            builder.HasOne(e => e.Offer)
                .WithMany(o => o.OfferCombinationPricings)
                .HasForeignKey(e => e.OfferId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TbOfferCombinationPricing_TbOffer");

            builder.HasOne(e => e.ItemCombination)
                .WithMany()
                .HasForeignKey(e => e.ItemCombinationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TbOfferCombinationPricing_TbItemCombination");

            // ============================================================================
            // Performance Indexes (created in migration)
            // ============================================================================

            // Note: Indexes are created in the migration 20251209162748_OptimizeItemSearchPerformance
            // They include:
            // - IX_TbOfferCombinationPricing_SalesPrice (price filtering)
            // - IX_TbOfferCombinationPricing_OfferId_SalesPrice (composite)
            // - IX_TbOfferCombinationPricing_StockStatus_AvailableQuantity (stock filtering)
            // - IX_TbOfferCombinationPricing_IsDefault (with INCLUDE columns)
        }
    }

    /// <summary>
    /// Fluent API configuration for TbOffer entity
    /// This configuration includes column setup and foreign key configuration
    /// NOTE: Indexes are created in the migration, not here
    /// </summary>
    public class OfferConfiguration : IEntityTypeConfiguration<TbOffer>
    {
        public void Configure(EntityTypeBuilder<TbOffer> builder)
        {
            // Primary key
            builder.HasKey(e => e.Id);

            // Table name
            builder.ToTable("TbOffers");

            // ============================================================================
            // Column Configurations
            // ============================================================================

            // Required foreign keys
            builder.Property(e => e.ItemId)
                .IsRequired();

            builder.Property(e => e.VendorId)
                .IsRequired();

            // Enumerations
            builder.Property(e => e.StorgeLocation)
                .IsRequired();

            builder.Property(e => e.VisibilityScope)
                .IsRequired();

            builder.Property(e => e.FulfillmentType)
                .IsRequired()
                .HasDefaultValue(FulfillmentType.Seller);

            // Time-based properties
            builder.Property(e => e.HandlingTimeInDays)
                .IsRequired()
                .HasDefaultValue(0);

            // Optional properties
            builder.Property(e => e.WarrantyId)
                .IsRequired(false);

            builder.Property(e => e.OfferConditionId)
                .IsRequired(false);

            // ============================================================================
            // Foreign Key Relationships
            // ============================================================================

            builder.HasOne(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TbOffers_TbItems");

            builder.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TbOffers_TbVendors");

            builder.HasOne(e => e.Warranty)
                .WithMany()
                .HasForeignKey(e => e.WarrantyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TbOffers_TbWarranties");

            builder.HasOne(e => e.OfferCondition)
                .WithMany()
                .HasForeignKey(e => e.OfferConditionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TbOffers_TbOfferConditions");

            // ============================================================================
            // Performance Indexes (created in migration)
            // ============================================================================

            // Note: Indexes are created in the migration 20251209162748_OptimizeItemSearchPerformance
            // They include:
            // - IX_TbOffers_ItemId_UserId (composite) - Maps to ItemId + VendorId
            // - IX_TbOffers_UserId (vendor filtering) - Maps to VendorId
            // - IX_TbOffers_VisibilityScope (visibility filtering)
            // - IX_TbOffers_StorgeLocation (location filtering)
            // - IX_TbOffers_ItemId (item lookup)
        }
    }

    /// <summary>
    /// Fluent API configuration for TbItem entity
    /// This configuration includes column setup and indexes for search operations
    /// NOTE: Indexes are created in the migration, not here
    /// </summary>
    public class ItemSearchConfiguration : IEntityTypeConfiguration<TbItem>
    {
        public void Configure(EntityTypeBuilder<TbItem> builder)
        {
            // Primary key (if not already configured)
            builder.HasKey(e => e.Id);

            // Table name
            builder.ToTable("TbItems");

            // ============================================================================
            // Column Configurations
            // ============================================================================

            // String columns with length constraints
            builder.Property(e => e.TitleAr)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.TitleEn)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.ShortDescriptionAr)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.ShortDescriptionEn)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.DescriptionAr)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.DescriptionEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.ThumbnailImage)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.VideoUrl)
                .HasMaxLength(200);

            // Numeric columns
            builder.Property(e => e.BasePrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.MinimumPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.MaximumPrice)
                .HasColumnType("decimal(18,2)");

            // Foreign keys
            builder.Property(e => e.CategoryId)
                .IsRequired();

            builder.Property(e => e.BrandId)
                .IsRequired();

            builder.Property(e => e.UnitId)
                .IsRequired();

            builder.Property(e => e.VideoProviderId)
                .IsRequired(false);

            // Status and visibility
            builder.Property(e => e.VisibilityScope)
                .IsRequired()
                .HasDefaultValue(0);

            // ============================================================================
            // Foreign Key Relationships
            // ============================================================================

            builder.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TbItems_TbCategories");

            builder.HasOne(e => e.Brand)
                .WithMany()
                .HasForeignKey(e => e.BrandId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TbItems_TbBrands");

            builder.HasOne(e => e.Unit)
                .WithMany()
                .HasForeignKey(e => e.UnitId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TbItems_TbUnits");

            // ============================================================================
            // Performance Indexes (created in migration)
            // ============================================================================

            // Note: Indexes are created in the migration 20251209162748_OptimizeItemSearchPerformance
            // They include:
            // - IX_TbItems_TitleAr (text search - Arabic)
            // - IX_TbItems_TitleEn (text search - English)
            // - IX_TbItems_CategoryId_BrandId (composite filtering)
            // - IX_TbItems_IsActive_CreatedDate (activity + sorting) - Inherited from BaseSeo
            // - IX_TbItems_CategoryId (category filtering)
            // - IX_TbItems_BrandId (brand filtering)
            // - IX_TbItems_CreatedDateUtc (date sorting)
            // - IX_TbItems_IsActive (activity filtering)
        }
    }
}
