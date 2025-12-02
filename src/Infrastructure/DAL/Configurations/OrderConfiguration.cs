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
        }
    }
}
