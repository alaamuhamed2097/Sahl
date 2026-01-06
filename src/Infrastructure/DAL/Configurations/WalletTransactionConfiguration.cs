using Domains.Entities.Wallet.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbCustomerWalletTransaction
    /// </summary>
    public class WalletTransactionConfiguration : IEntityTypeConfiguration<TbCustomerWalletTransaction>
    {
        public void Configure(EntityTypeBuilder<TbCustomerWalletTransaction> entity)
        {
            // Table name
            entity.ToTable("TbWalletTransactions");

            // Property configurations
            entity.Property(e => e.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.FeeAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.Direction)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.TransactionType)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.TransactionStatus)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.ReferenceType)
                .IsRequired()
                .HasMaxLength(50); 

            entity.Property(e => e.ReferenceId)
                .IsRequired();

            // Relationships
            entity.HasOne(e => e.Wallet)
                .WithMany(w => w.Transactions)
                .HasForeignKey(e => e.WalletId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.WalletId);
            entity.HasIndex(e => e.TransactionType);
            entity.HasIndex(e => e.TransactionStatus);
            entity.HasIndex(e => e.ReferenceId);
        }
    }
}
