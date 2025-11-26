using Domains.Entities.Pricing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbPriceHistory
    /// </summary>
    public class PriceHistoryConfiguration : IEntityTypeConfiguration<TbPriceHistory>
    {
        public void Configure(EntityTypeBuilder<TbPriceHistory> entity)
        {
            // Table name
            entity.ToTable("TbPriceHistories");

            // Property configurations
            entity.Property(e => e.OldPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.NewPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.ChangedPercentage)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.ChangedAt)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.Reason)
                .HasMaxLength(500);

            entity.Property(e => e.IsAutomatic)
                .HasDefaultValue(false);

            // Relationships
            entity.HasOne(e => e.Offer)
                .WithMany()
                .HasForeignKey(e => e.OfferId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ChangedByUser)
                .WithMany()
                .HasForeignKey(e => e.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.OfferId);

            entity.HasIndex(e => e.ChangedAt);

            entity.HasIndex(e => e.IsAutomatic);

            entity.HasIndex(e => new { e.OfferId, e.ChangedAt });
        }
    }
}
