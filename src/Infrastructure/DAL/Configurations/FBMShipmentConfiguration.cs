using Domains.Entities.Fulfillment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbFBMShipment
    /// </summary>
    public class FBMShipmentConfiguration : IEntityTypeConfiguration<TbFBMShipment>
    {
        public void Configure(EntityTypeBuilder<TbFBMShipment> entity)
        {
            // Table name
            entity.ToTable("TbFBMShipments");

            // Property configurations
            entity.Property(e => e.ShipmentNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.TrackingNumber)
                .HasMaxLength(100);

            entity.Property(e => e.PickupDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.ShippedDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.DeliveredDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.ShippingCost)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.ActualWeight)
                .HasColumnType("decimal(10,3)");

            entity.Property(e => e.VolumetricWeight)
                .HasColumnType("decimal(10,3)");

            entity.Property(e => e.Notes)
                .HasMaxLength(1000);

            entity.Property(e => e.DeliveryNotes)
                .HasMaxLength(1000);

            // Relationships
            entity.HasOne(e => e.Order)
                .WithMany()
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Warehouse)
                .WithMany()
                .HasForeignKey(e => e.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ShippingCompany)
                .WithMany()
                .HasForeignKey(e => e.ShippingCompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.ShipmentNumber)
                .IsUnique();

            entity.HasIndex(e => e.OrderId);

            entity.HasIndex(e => e.WarehouseId);

            entity.HasIndex(e => e.ShippingCompanyId);

            entity.HasIndex(e => e.Status);

            entity.HasIndex(e => e.TrackingNumber);

            entity.HasIndex(e => e.ShippedDate);

            entity.HasIndex(e => new { e.Status, e.ShippedDate });
        }
    }
}
