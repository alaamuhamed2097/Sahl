using Domains.Entities.Offer;
using Domains.Entities.Offer.Warranty;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class OfferConfiguration : IEntityTypeConfiguration<TbOffer>
    {
        public void Configure(EntityTypeBuilder<TbOffer> builder)
        {
            builder.HasKey(o => o.Id);
            builder.ToTable("TbOffers");

            // ============================================================================
            // Column Configurations
            // ============================================================================
            builder.Property(e => e.ItemId).IsRequired();
            builder.Property(e => e.VendorId).IsRequired();
            builder.Property(e => e.VisibilityScope).IsRequired();
           


            // ============================================================================
            // STRATEGIC INDEXES FOR MULTI-VENDOR QUERIES
            // ============================================================================

            // ✅ 1. COMPOSITE COVERING INDEX for Item → Offers lookup
            // Usage: JOIN TbOffers ON ItemId WHERE VisibilityScope = 1 AND IsDeleted = 0
            // This is the MOST CRITICAL index for search performance
            // INCLUDE: All columns needed in SELECT to avoid key lookups
            builder.HasIndex(e => new { e.ItemId, e.VisibilityScope, e.IsDeleted })
                .HasDatabaseName("IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC")
                .IsUnique(false)
                .IncludeProperties(e => new
                {
                    e.VendorId,
                    e.HandlingTimeInDays,
                    e.FulfillmentType
                });

            // ✅ 2. FILTERED INDEX for Public Offers Only
            // Usage: WHERE VisibilityScope = 1 (Public)
            // Reduces index size significantly (excludes private/draft offers)
            builder.HasIndex(e => new { e.VisibilityScope, e.IsDeleted })
                .HasDatabaseName("IX_TbOffers_Public_Filtered_NC")
                .IsUnique(false)
                .HasFilter("[VisibilityScope] = 1 AND [IsDeleted] = 0");

            // ✅ 3. NONCLUSTERED INDEX for Vendor Filtering
            // Usage: WHERE VendorId IN (...)
            builder.HasIndex(e => e.VendorId)
                .HasDatabaseName("IX_TbOffers_VendorId_NC")
                .IsUnique(false);


            // ✅ 5. COMPOSITE INDEX for Vendor + Item (Unique Constraint)
            // Usage: Ensure one offer per vendor per item (business rule)
            builder.HasIndex(e => new { e.VendorId, e.ItemId })
                .HasDatabaseName("IX_TbOffers_VendorId_ItemId_Unique")
                .IsUnique(true)
                .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(o => o.WarrantyId)
                   .HasDatabaseName("IX_TbOffers_WarrantyId");

            // ============================================================================
            // Foreign Key Relationships
            // ============================================================================

            builder.HasOne(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Offer -> Warranty (many offers can reference a warranty)
            builder.HasOne<TbWarranty>(o => o.Warranty)
                   .WithMany(w => w.OffersList)
                   .HasForeignKey(o => o.WarrantyId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict); // ✅ CHANGED from SetNull

            // Collections - CRITICAL: Change all Cascade to Restrict
            builder.HasMany(o => o.UserOfferRatings)
                   .WithOne(r => r.Offer)
                   .HasForeignKey(r => r.OfferId)
                   .OnDelete(DeleteBehavior.Restrict); // ✅ CHANGED from Cascade

            builder.HasMany(o => o.ShippingDetails)
                   .WithOne(s => s.Offer)
                   .HasForeignKey(s => s.OfferId)
                   .OnDelete(DeleteBehavior.Restrict); // ✅ CHANGED from Cascade

            builder.HasMany(o => o.OfferCombinationPricings)
                   .WithOne(c => c.Offer)
                   .HasForeignKey(c => c.OfferId)
                   .OnDelete(DeleteBehavior.Restrict); // ✅ CHANGED from Cascade

            // Query filter for active offers
            builder.HasQueryFilter(o => !o.IsDeleted);
        }
    }
}