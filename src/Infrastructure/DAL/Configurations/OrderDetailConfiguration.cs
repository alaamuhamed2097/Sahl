using Domains.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<TbOrderDetail>
    {
        public void Configure(EntityTypeBuilder<TbOrderDetail> builder)
        {
            builder.ToTable("TbOrderDetails");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.SubTotal)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.DiscountAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TaxAmount)
                .HasColumnType("decimal(18,2)");

            // Foreign Key Relationships
            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Item)
                .WithMany()
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.OfferCombinationPricing)
                .WithMany()
                .HasForeignKey(x => x.OfferCombinationPricingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Vendor)
                .WithMany()
                .HasForeignKey(x => x.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(e => e.OrderId).HasDatabaseName("IX_OrderDetails_OrderId");
            builder.HasIndex(e => e.ItemId).HasDatabaseName("IX_OrderDetails_ItemId");
            builder.HasIndex(e => e.OfferCombinationPricingId).HasDatabaseName("IX_OrderDetails_OfferCombinationPricingId");
            builder.HasIndex(e => e.VendorId).HasDatabaseName("IX_OrderDetails_VendorId");
            builder.HasIndex(e => e.WarehouseId).HasDatabaseName("IX_OrderDetails_WarehouseId");
        }
    }
}
