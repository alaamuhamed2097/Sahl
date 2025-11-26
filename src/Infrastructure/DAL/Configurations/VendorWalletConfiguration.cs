using Domains.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbVendorWallet
    /// </summary>
    public class VendorWalletConfiguration : IEntityTypeConfiguration<TbVendorWallet>
    {
        public void Configure(EntityTypeBuilder<TbVendorWallet> entity)
        {
            // Table name
            entity.ToTable("TbVendorWallets");

            // Property configurations
            entity.Property(e => e.AvailableBalance)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.PendingBalance)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.TotalEarned)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.TotalWithdrawn)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.TotalCommissionPaid)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.WithdrawalFeePercentage)
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(2.5m);

            entity.Property(e => e.LastWithdrawalDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.LastTransactionDate)
                .HasColumnType("datetime2(2)");

            // Relationships
            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.VendorId)
                .IsUnique();

            entity.HasIndex(e => e.CurrencyId);

            entity.HasIndex(e => e.AvailableBalance);
        }
    }
}
