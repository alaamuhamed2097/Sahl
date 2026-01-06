using Domains.Entities.Order.Refund;
using Domains.Entities.SellerRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbRefund
    /// </summary>
    public class RefundConfiguration : IEntityTypeConfiguration<TbRefund>
    {
        public void Configure(EntityTypeBuilder<TbRefund> entity)
        {
            // Table name
            entity.ToTable("TbRefunds");

            // Primary Key
            entity.HasKey(e => e.Id);

            // Refund Number
            entity.Property(e => e.Number)
                  .IsRequired()
                  .HasMaxLength(50);

            // Required Fields
            entity.Property(e => e.OrderDetailId).IsRequired();
            entity.Property(e => e.CustomerId).IsRequired();
            entity.Property(e => e.DeliveryAddressId).IsRequired();
            entity.Property(e => e.VendorId).IsRequired();

            // Enums
            entity.Property(e => e.RefundReason)
                  .IsRequired()
                  .HasConversion<int>();

            entity.Property(e => e.RefundStatus)
                  .IsRequired()
                  .HasConversion<int>();

            // Strings
            entity.Property(e => e.RefundReasonDetails)
                  .HasMaxLength(1000);

            entity.Property(e => e.RejectionReason)
                  .HasMaxLength(1000);

            entity.Property(e => e.AdminNotes)
                  .HasMaxLength(1000);

            entity.Property(e => e.AdminUserId)
                  .HasMaxLength(450);

            // Decimals
            entity.Property(e => e.ReturnShippingCost)
                  .HasColumnType("decimal(18,2)")
                  .HasDefaultValue(0m);

            entity.Property(e => e.RefundAmount)
                  .HasColumnType("decimal(18,2)")
                  .HasDefaultValue(0m);

            // Dates
            entity.Property(e => e.RequestDateUTC)
                  .IsRequired();

            // Relationships

            entity.HasOne(e => e.OrderDetail)
                  .WithMany(o=>o.Refunds)
                  .HasForeignKey(e => e.OrderDetailId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.Refunds)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CustomerAddress)
                  .WithMany(c => c.Refunds)
                  .HasForeignKey(e => e.DeliveryAddressId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Vendor)
                  .WithMany(v => v.Refunds)
                  .HasForeignKey(e => e.VendorId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.AdminUser)
                  .WithMany(c => c.Refunds)
                  .HasForeignKey(e => e.AdminUserId)
                  .OnDelete(DeleteBehavior.SetNull);

            // Collections
            entity.HasMany(e => e.RefundItemVideos)
                  .WithOne(r => r.Refund)
                  .HasForeignKey(x => x.RefundId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.RefundStatusHistories)
                  .WithOne(r => r.Refund)
                  .HasForeignKey(x => x.RefundId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.Number).IsUnique(true);

            entity.HasIndex(e => e.OrderDetailId);

            entity.HasIndex(e => e.VendorId);

            entity.HasIndex(e => e.CustomerId);
           
            entity.HasIndex(e => new { e.CustomerId, e.RefundStatus });
            
            entity.HasIndex(e => e.RequestDateUTC);

            entity.HasIndex(e => new { e.VendorId, e.RefundStatus });

            // Concurrency Token
            entity.Property<byte[]>("RowVersion")
            .IsRowVersion();

            // Check Constraints
            entity.ToTable("TbRefunds", tb =>
            {
                tb.HasCheckConstraint("CK_Refund_Amounts",
                    "[RefundAmount] >= 0 AND [ReturnShippingCost] >= 0");

                tb.HasCheckConstraint("CK_Refund_ItemsCount",
                    "[ApprovedItemsCount] <= [RequestedItemsCount]");
            });
        }
    }
}
