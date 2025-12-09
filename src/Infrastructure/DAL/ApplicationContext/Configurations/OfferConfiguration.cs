using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Offer;
using Domains.Entities.Offer.Warranty;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.ApplicationContext.Configurations
{
    public class OfferConfiguration : IEntityTypeConfiguration<TbOffer>
    {
        public void Configure(EntityTypeBuilder<TbOffer> builder)
        {
            builder.ToTable("TbOffers");

            builder.HasKey(o => o.Id);

            // Offer -> Item (many-to-one)
            builder.HasOne<TbItem>(o => o.Item)
                   .WithMany()
                   .HasForeignKey(o => o.ItemId)
                   .OnDelete(DeleteBehavior.Restrict); // ✅ CHANGED from ClientSetNull

            // Offer -> Vendor (many-to-one)
            builder.HasOne<TbVendor>(o => o.Vendor)
                   .WithMany()
                   .HasForeignKey(o => o.VendorId)
                   .OnDelete(DeleteBehavior.Restrict); // ✅ CHANGED from Cascade

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

            // Indexes
            builder.HasIndex(o => o.ItemId)
                   .HasDatabaseName("IX_TbOffers_ItemId");

            builder.HasIndex(o => o.VendorId)
                   .HasDatabaseName("IX_TbOffers_VendorId");

            builder.HasIndex(o => o.WarrantyId)
                   .HasDatabaseName("IX_TbOffers_WarrantyId");

            // Query filter for active offers
            builder.HasQueryFilter(o => !o.IsDeleted);
        }
    }
}