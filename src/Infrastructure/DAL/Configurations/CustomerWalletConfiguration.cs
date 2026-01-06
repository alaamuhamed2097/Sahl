using Domains.Entities.Wallet.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbCustomerWallet
    /// </summary>
    public class CustomerWalletConfiguration : IEntityTypeConfiguration<TbCustomerWallet>
    {
        public void Configure(EntityTypeBuilder<TbCustomerWallet> entity)
        {
            // Table name
            entity.ToTable("TbCustomerWallets");

            // Property configurations
            entity.Property(e => e.Balance)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.PendingBalance)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.LockedBalance)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.LastTransactionDate)
                .HasColumnType("datetime2(2)");

            // Relationships
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.UserId)
                .IsUnique();

            entity.HasIndex(e => e.Balance);
            entity.HasIndex(e => e.LockedBalance);
        }
    }
}
