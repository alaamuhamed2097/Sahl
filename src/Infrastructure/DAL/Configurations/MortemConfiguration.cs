using Domains.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class MortemConfiguration : IEntityTypeConfiguration<TbMortem>
    {
        public void Configure(EntityTypeBuilder<TbMortem> builder)
        {
            builder.ToTable("TbMortems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.DocumentNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.DocumentDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.Reason)
                .HasMaxLength(500);

            builder.Property(x => x.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Status)
                .HasDefaultValue(0);

            builder.Property(x => x.CreatedDateUtc)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.UpdatedDateUtc)
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.CurrentState)
                .HasDefaultValue(1);

            builder.HasOne(x => x.Order)
                .WithMany()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.CurrentState);
            builder.HasIndex(x => x.DocumentNumber);
            builder.HasIndex(x => x.Status);
        }
    }
}
