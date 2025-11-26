using Domains.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbPlatformTreasury
    /// </summary>
    public class PlatformTreasuryConfiguration : IEntityTypeConfiguration<TbPlatformTreasury>
    {
        public void Configure(EntityTypeBuilder<TbPlatformTreasury> entity)
        {
            // Table name
            entity.ToTable("TbPlatformTreasuries");

            // Property configurations
            entity.Property(e => e.TotalBalance)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.CustomerWalletsTotal)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.VendorWalletsTotal)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.PendingCommissions)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.CollectedCommissions)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.PendingPayouts)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.ProcessedPayouts)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.LastReconciliationDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.Notes)
                .HasMaxLength(500);

            // Relationships
            entity.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.CurrencyId);

            entity.HasIndex(e => e.LastReconciliationDate);
        }
    }
}
