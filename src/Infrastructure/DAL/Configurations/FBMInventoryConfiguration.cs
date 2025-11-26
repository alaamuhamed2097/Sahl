using Domains.Entities.Fulfillment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbFBMInventory
    /// </summary>
    public class FBMInventoryConfiguration : IEntityTypeConfiguration<TbFBMInventory>
    {
        public void Configure(EntityTypeBuilder<TbFBMInventory> entity)
        {
            // Table name
            entity.ToTable("TbFBMInventories");

            // Property configurations
            entity.Property(e => e.AvailableQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(e => e.ReservedQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(e => e.InTransitQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(e => e.DamagedQuantity)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(e => e.LastSyncDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.SKU)
                .HasMaxLength(100);

            entity.Property(e => e.LocationCode)
                .HasMaxLength(100);

            entity.Property(e => e.ExpiryDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.Notes)
                .HasMaxLength(500);

            // Relationships
            entity.HasOne(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Warehouse)
                .WithMany()
                .HasForeignKey(e => e.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.ItemId);

            entity.HasIndex(e => e.VendorId);

            entity.HasIndex(e => e.WarehouseId);

            entity.HasIndex(e => e.SKU);

            entity.HasIndex(e => new { e.ItemId, e.WarehouseId, e.VendorId })
                .IsUnique();

            entity.HasIndex(e => new { e.WarehouseId, e.AvailableQuantity });
        }
    }
}
