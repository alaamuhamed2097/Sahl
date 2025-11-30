using Domains.Entities.Wallet;
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

            entity.Property(e => e.TotalSpent)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.LastTransactionDate)
                .HasColumnType("datetime2(2)");

            // Relationships
            entity.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.CustomerId)
                .IsUnique();

            entity.HasIndex(e => e.CurrencyId);

            entity.HasIndex(e => e.AvailableBalance);
        }
    }
}
