using Domains.Entities.BuyBox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbBuyBoxHistory
    /// </summary>
    public class BuyBoxHistoryConfiguration : IEntityTypeConfiguration<TbBuyBoxHistory>
    {
        public void Configure(EntityTypeBuilder<TbBuyBoxHistory> entity)
        {
            // Table name
            entity.ToTable("TbBuyBoxHistories");

            // Property configurations
            entity.Property(e => e.Score)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.WonAt)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.LostAt)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.DurationInMinutes)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(e => e.LossReason)
                .HasMaxLength(500);

            // Relationships
            entity.HasOne(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Offer)
                .WithMany()
                .HasForeignKey(e => e.OfferId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.ItemId);

            entity.HasIndex(e => e.VendorId);

            entity.HasIndex(e => e.WonAt);

            entity.HasIndex(e => e.LostAt);

            entity.HasIndex(e => new { e.ItemId, e.WonAt });

            entity.HasIndex(e => new { e.VendorId, e.WonAt });
        }
    }
}
