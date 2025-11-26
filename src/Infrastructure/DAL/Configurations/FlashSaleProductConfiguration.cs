using Domains.Entities.Campaign;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbFlashSaleProduct
    /// </summary>
    public class FlashSaleProductConfiguration : IEntityTypeConfiguration<TbFlashSaleProduct>
    {
        public void Configure(EntityTypeBuilder<TbFlashSaleProduct> entity)
        {
            // Table name
            entity.ToTable("TbFlashSaleProducts");

            // Property configurations
            entity.Property(e => e.OriginalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.FlashSalePrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.DiscountPercentage)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.AvailableQuantity)
                .IsRequired();

            entity.Property(e => e.SoldQuantity)
                .HasDefaultValue(0);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0);

            entity.Property(e => e.ViewCount)
                .HasDefaultValue(0);

            entity.Property(e => e.AddToCartCount)
                .HasDefaultValue(0);

            // Relationships
            entity.HasOne(e => e.FlashSale)
                .WithMany(f => f.FlashSaleProducts)
                .HasForeignKey(e => e.FlashSaleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.FlashSaleId);

            entity.HasIndex(e => e.ItemId);

            entity.HasIndex(e => e.VendorId);

            entity.HasIndex(e => e.IsActive);

            entity.HasIndex(e => e.DisplayOrder);

            entity.HasIndex(e => new { e.FlashSaleId, e.ItemId })
                .IsUnique();

            entity.HasIndex(e => new { e.FlashSaleId, e.SoldQuantity });
        }
    }
}
