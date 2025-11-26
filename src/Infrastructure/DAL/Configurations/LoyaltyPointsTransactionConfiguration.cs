using Domains.Entities.Loyalty;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbLoyaltyPointsTransaction
    /// </summary>
    public class LoyaltyPointsTransactionConfiguration : IEntityTypeConfiguration<TbLoyaltyPointsTransaction>
    {
        public void Configure(EntityTypeBuilder<TbLoyaltyPointsTransaction> entity)
        {
            // Table name
            entity.ToTable("TbLoyaltyPointsTransactions");

            // Property configurations
            entity.Property(e => e.Points)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.TransactionType)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.DescriptionEn)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.DescriptionAr)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.ExpiryDate)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.IsExpired)
                .HasDefaultValue(false);

            entity.Property(e => e.Notes)
                .HasMaxLength(500);

            // Relationships
            entity.HasOne(e => e.CustomerLoyalty)
                .WithMany(c => c.PointsTransactions)
                .HasForeignKey(e => e.CustomerLoyaltyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Order)
                .WithMany()
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Review)
                .WithMany()
                .HasForeignKey(e => e.ReviewId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.CustomerLoyaltyId);

            entity.HasIndex(e => e.TransactionType);

            entity.HasIndex(e => e.CreatedDateUtc);

            entity.HasIndex(e => e.IsExpired);

            entity.HasIndex(e => new { e.CustomerLoyaltyId, e.TransactionType });
        }
    }
}
