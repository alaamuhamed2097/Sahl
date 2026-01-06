using Domains.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<TbOrder>
    {
        public void Configure(EntityTypeBuilder<TbOrder> builder)
        {
            builder.ToTable("TbOrders");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Number)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            // Indexes
            builder.HasIndex(e => e.UserId).HasDatabaseName("IX_Orders_CustomerId");
            builder.HasIndex(e => e.OrderStatus).HasDatabaseName("IX_Orders_Status");
            builder.HasIndex(e => e.CreatedDateUtc).HasDatabaseName("IX_Orders_Date");
            builder.HasIndex(e => e.Number).HasDatabaseName("IX_Orders_Number");

            // Foreign Keys - تعديل العلاقات لتجنب CASCADE DELETE المتعدد
            builder.HasOne(o => o.User)
                .WithMany(u=> u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TbOrders_AspNetUsers_UserId");

            builder.HasOne(o => o.CustomerAddress)
                .WithMany(c=> c.Orders)
                .HasForeignKey(o => o.DeliveryAddressId)
                .OnDelete(DeleteBehavior.NoAction) // تغيير هنا
                .HasConstraintName("FK_TbOrders_TbCustomerAddresses_DeliveryAddressId");

            builder.HasOne(o => o.Coupon)
                .WithMany(c=>c.Orders)
                .HasForeignKey(o => o.CouponId)
                .OnDelete(DeleteBehavior.NoAction) // تغيير هنا
                .HasConstraintName("FK_TbOrders_TbCouponCodes_CouponId");
        }
    }
}