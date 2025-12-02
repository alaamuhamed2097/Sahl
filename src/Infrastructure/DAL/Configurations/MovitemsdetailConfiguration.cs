using Domains.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class MovitemsdetailConfiguration : IEntityTypeConfiguration<TbMovitemsdetail>
    {
        public void Configure(EntityTypeBuilder<TbMovitemsdetail> builder)
        {
            builder.ToTable("TbMovitemsdetails");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.CreatedDateUtc)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.UpdatedDateUtc)
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.CurrentState)
                .HasDefaultValue(1);

            builder.HasOne(x => x.Moitem)
                .WithMany(m => m.MovitemsDetails)
                .HasForeignKey(x => x.MoitemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Mortem)
                .WithMany(m => m.MovitemsDetails)
                .HasForeignKey(x => x.MortemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Item)
                .WithMany()
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Warehouse)
                .WithMany(w => w.MovitemsDetails)
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.ItemCombination)
                .WithMany()
                .HasForeignKey(x => x.ItemCombinationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.CurrentState);
            builder.HasIndex(x => x.ItemId);
            builder.HasIndex(x => x.WarehouseId);
        }
    }
}
