using Domains.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbWalletTransaction
    /// </summary>
    public class WalletTransactionConfiguration : IEntityTypeConfiguration<TbWalletTransaction>
    {
        public void Configure(EntityTypeBuilder<TbWalletTransaction> entity)
        {
            // Table name
            entity.ToTable("TbWalletTransactions");

            // Property configurations
            entity.Property(e => e.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.TransactionType)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.DescriptionEn)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.DescriptionAr)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.ReferenceNumber)
                .HasMaxLength(100);

            entity.Property(e => e.BalanceBefore)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.BalanceAfter)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.Notes)
                .HasMaxLength(500);

            entity.Property(e => e.ProcessedDate)
                .HasColumnType("datetime2(2)");

            // Relationships
            entity.HasOne(e => e.CustomerWallet)
                .WithMany(w => w.Transactions)
                .HasForeignKey(e => e.CustomerWalletId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.VendorWallet)
                .WithMany(w => w.Transactions)
                .HasForeignKey(e => e.VendorWalletId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Order)
                .WithMany()
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Refund)
                .WithMany()
                .HasForeignKey(e => e.RefundId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ProcessedByUser)
                .WithMany()
                .HasForeignKey(e => e.ProcessedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.CustomerWalletId);

            entity.HasIndex(e => e.VendorWalletId);

            entity.HasIndex(e => e.TransactionType);

            entity.HasIndex(e => e.Status);

            entity.HasIndex(e => e.CreatedDateUtc);

            entity.HasIndex(e => e.ReferenceNumber)
                .IsUnique()
                .HasFilter("[ReferenceNumber] IS NOT NULL");

            entity.HasIndex(e => new { e.CustomerWalletId, e.Status });

            entity.HasIndex(e => new { e.VendorWalletId, e.Status });
        }
    }
}
