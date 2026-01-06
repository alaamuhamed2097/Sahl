using Domains.Entities.Wallet.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbWalletSetting
    /// </summary>
    public class WalletSettingConfiguration : IEntityTypeConfiguration<TbWalletSetting>
    {
        public void Configure(EntityTypeBuilder<TbWalletSetting> entity)
        {
            // Table name
            entity.ToTable("TbWalletSettings");

            // Property configurations
            entity.Property(e => e.MinChargingAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(10m);

            entity.Property(e => e.MaxChargingAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(10000m);

            entity.Property(e => e.MaxDailyChargingLimit)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(50000m);

            entity.Property(e => e.ChargingFeePercentage)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.ChargingFeeFixed)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            entity.Property(e => e.IsChargingEnabled)
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(e => e.IsPaymentEnabled)
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(e => e.IsTransferEnabled)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
