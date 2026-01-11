using Domains.Entities.Order.Shipping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class OrderShipmentConfiguration : IEntityTypeConfiguration<TbOrderShipment>
    {
        public void Configure(EntityTypeBuilder<TbOrderShipment> builder)
        {
            builder.ToTable("TbOrderShipments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Number)
                .IsRequired();

            // Configure relationship with shipment items
            builder.HasMany(x => x.Items)
                .WithOne(x => x.Shipment)
                .HasForeignKey(x => x.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(e => e.OrderId).HasDatabaseName("IX_OrderShipments_OrderId");
            builder.HasIndex(e => e.VendorId).HasDatabaseName("IX_OrderShipments_VendorId");
            builder.HasIndex(e => e.ShipmentStatus).HasDatabaseName("IX_OrderShipments_Status");
            builder.HasIndex(e => e.Number).HasDatabaseName("IX_OrderShipments_Number");
        }
    }
}
