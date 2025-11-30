using Domains.Entities.Warehouse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<TbWarehouse>
    {
        public void Configure(EntityTypeBuilder<TbWarehouse> builder)
        {
            builder.ToTable("TbWarehouses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.TitleAr)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.TitleEn)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Address)
                .HasMaxLength(500);

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(x => x.PhoneCode)
                .HasMaxLength(4);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedDateUtc)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.UpdatedDateUtc)
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.CurrentState)
                .HasDefaultValue(1);

            builder.HasIndex(x => x.CurrentState);
            builder.HasIndex(x => x.IsActive);
        }
    }
}
