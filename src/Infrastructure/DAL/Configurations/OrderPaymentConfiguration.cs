using Domains.Entities.Order.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class OrderPaymentConfiguration : IEntityTypeConfiguration<TbOrderPayment>
    {
        public void Configure(EntityTypeBuilder<TbOrderPayment> builder)
        {
            builder.ToTable("TbOrderPayments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.RefundAmount)
                .HasColumnType("decimal(18,2)");

            // Relationships
            builder.HasOne(x => x.Order)
                .WithMany()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PaymentMethod)
                .WithMany()
                .HasForeignKey(x => x.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(e => e.OrderId).HasDatabaseName("IX_OrderPayments_OrderId");
            builder.HasIndex(e => e.PaymentStatus).HasDatabaseName("IX_OrderPayments_Status");
            builder.HasIndex(e => e.PaymentMethodId).HasDatabaseName("IX_OrderPayments_PaymentMethodId");
        }
    }
}
