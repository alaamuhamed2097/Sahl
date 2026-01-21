using Domains.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ShipmentPaymentConfiguration : IEntityTypeConfiguration<TbShipmentPayment>
    {
        public void Configure(EntityTypeBuilder<TbShipmentPayment> builder)
        {
            builder.ToTable("TbShipmentPayments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

            // Relationships
            builder.HasOne(x => x.Shipment)
                .WithMany() // or .WithMany(s => s.Payments) if you have the collection
                .HasForeignKey(x => x.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade); // When shipment deleted, payment deleted

            builder.HasOne(x => x.Order)
                .WithMany() // or .WithMany(o => o.ShipmentPayments) if you have the collection
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent order deletion if payments exist

            // Indexes
            builder.HasIndex(e => e.ShipmentId).HasDatabaseName("IX_ShipmentPayments_ShipmentId");
            builder.HasIndex(e => e.OrderId).HasDatabaseName("IX_ShipmentPayments_OrderId");
            builder.HasIndex(e => e.PaymentStatus).HasDatabaseName("IX_ShipmentPayments_Status");
            builder.HasIndex(e => e.IsDeleted).HasDatabaseName("IX_ShipmentPayments_IsDeleted");
        }
    }
}