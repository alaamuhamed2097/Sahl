using Domains.Entities.Wallet.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbWalletChargingRequest
    /// </summary>
    public class WalletChargingRequestConfiguration : IEntityTypeConfiguration<TbWalletChargingRequest>
    {
        public void Configure(EntityTypeBuilder<TbWalletChargingRequest> entity)
        {
            // Table name
            entity.ToTable("TbWalletChargingRequests");

            // Property configurations
            entity.Property(e => e.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.GatewayTransactionId)
                .HasMaxLength(100);

            entity.Property(e => e.FailureReason)
                .HasMaxLength(500);

            // Relationships
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.PaymentMethodId);
        }
    }
}
