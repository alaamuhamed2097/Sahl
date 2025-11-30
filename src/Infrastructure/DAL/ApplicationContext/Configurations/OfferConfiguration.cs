using Domains.Entities.Catalog.Item;
using Domains.Entities.Offer;
using Domains.Entities.Offer.Warranty;
using Domains.Identity;
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

            // Property configurations
            builder.Property(o => o.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.OriginalPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Quantity)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(o => o.ShippingCost)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.Property(o => o.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(o => o.Condition)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.SellerRating)
                .HasColumnType("decimal(3,2)");

            // CRITICAL FIX: Change all Cascade/ClientSetNull to Restrict
            // Offer -> Item (many-to-one)
            builder.HasOne<TbItem>(o => o.Item)
                   .WithMany()
                   .HasForeignKey(o => o.ItemId)
                   .OnDelete(DeleteBehavior.Restrict); // ✅ CHANGED from ClientSetNull

            // Offer -> User (many-to-one)
            builder.HasOne<ApplicationUser>(o => o.User)
                   .WithMany()
                   .HasForeignKey(o => o.UserId)
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

            builder.HasIndex(o => o.UserId)
                   .HasDatabaseName("IX_TbOffers_UserId");

            builder.HasIndex(o => o.WarrantyId)
                   .HasDatabaseName("IX_TbOffers_WarrantyId");

            builder.HasIndex(o => o.IsActive)
                   .HasDatabaseName("IX_TbOffers_IsActive");

            builder.HasIndex(o => o.Price)
                   .HasDatabaseName("IX_TbOffers_Price");

            builder.HasIndex(o => new { o.ItemId, o.IsActive, o.Price })
                   .HasDatabaseName("IX_TbOffers_Item_Active_Price");

            // Query filter for active offers
            builder.HasQueryFilter(o => o.IsActive && o.CurrentState == 1);
        }
    }
}