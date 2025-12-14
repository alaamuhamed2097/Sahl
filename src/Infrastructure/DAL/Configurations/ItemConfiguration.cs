using Domains.Entities.Catalog.Item;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbItem
    /// </summary>
    public class ItemConfiguration : IEntityTypeConfiguration<TbItem>
    {
        public void Configure(EntityTypeBuilder<TbItem> entity)
        {
            // Property configurations
            entity.Property(e => e.TitleAr)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(true);

            entity.Property(e => e.TitleEn)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(true);

            entity.Property(e => e.ShortDescriptionAr)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(true);

            entity.Property(e => e.ShortDescriptionEn)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(true);

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(500)
                .IsUnicode(true);

            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(500)
                .IsUnicode(true);

            entity.Property(e => e.ThumbnailImage)
               .IsRequired()
               .HasMaxLength(200);

            entity.Property(e => e.SEOTitle)
                   .IsRequired()
                   .HasMaxLength(100);

            entity.Property(e => e.SEOMetaTags)
                    .IsRequired()
                 .HasMaxLength(200);

            // ============================================================================
            // STRATEGIC INDEXES FOR LARGE DATASETS
            // ============================================================================

            // ✅ 1. NONCLUSTERED INDEX for Arabic Title Search
            // Usage: WHERE TitleAr LIKE '%keyword%'
            // Cardinality: HIGH (each product has unique title)
            // Read/Write Ratio: 95% Read / 5% Write
            entity.HasIndex(e => e.TitleAr)
                .HasDatabaseName("IX_TbItems_TitleAr_NC")
                .IsUnique(false);  // Non-clustered

            // ✅ 2. NONCLUSTERED INDEX for English Title Search
            entity.HasIndex(e => e.TitleEn)
                .HasDatabaseName("IX_TbItems_TitleEn_NC")
                .IsUnique(false);

            // ✅ 3. COMPOSITE NONCLUSTERED INDEX for Category + Brand Filtering
            // Usage: WHERE CategoryId = X AND BrandId = Y
            // Selectivity: HIGH (combination is very selective)
            // Most common filter combination in e-commerce
            entity.HasIndex(e => new { e.CategoryId, e.BrandId })
                .HasDatabaseName("IX_TbItems_CategoryId_BrandId_NC")
                .IsUnique(false);

            // ✅ 4. FILTERED NONCLUSTERED INDEX for Active Items Only
            // Usage: WHERE IsActive = 1 AND IsDeleted = 0
            // Reduces index size by ~30-50% (inactive items excluded)
            // INCLUDE columns for covering index (avoid key lookups)
            entity.HasIndex(e => new { e.IsActive, e.IsDeleted })
                .HasDatabaseName("IX_TbItems_Active_Filtered_NC")
                .IsUnique(false)
                .HasFilter("[IsActive] = 1 AND [IsDeleted] = 0")
                .IncludeProperties(e => new
                {
                    e.TitleAr,
                    e.TitleEn,
                    e.CategoryId,
                    e.BrandId,
                    e.ThumbnailImage,
                    e.CreatedDateUtc
                });

            // ✅ 5. NONCLUSTERED INDEX for CreatedDateUtc (Sorting)
            // Usage: ORDER BY CreatedDateUtc DESC
            // Sorted data structure for "newest first" queries
            entity.HasIndex(e => e.CreatedDateUtc)
                .HasDatabaseName("IX_TbItems_CreatedDateUtc_NC")
                .IsDescending()  // Optimized for DESC queries
                .IsUnique(false);

            // ✅ 6. NONCLUSTERED INDEX for CategoryId (Foreign Key)
            // Usage: Joins and WHERE CategoryId IN (...)
            entity.HasIndex(e => e.CategoryId)
                .HasDatabaseName("IX_TbItems_CategoryId_NC")
                .IsUnique(false);

            // ✅ 7. NONCLUSTERED INDEX for BrandId (Foreign Key)
            entity.HasIndex(e => e.BrandId)
                .HasDatabaseName("IX_TbItems_BrandId_NC")
                .IsUnique(false);

            // ============================================================================
            // Foreign Key Relationships
            // ============================================================================

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Brand)
                .WithMany()
                .HasForeignKey(e => e.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Unit)
                .WithMany()
                .HasForeignKey(e => e.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

