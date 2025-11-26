using Domains.Entities.Fulfillment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbFulfillmentFee
    /// </summary>
    public class FulfillmentFeeConfiguration : IEntityTypeConfiguration<TbFulfillmentFee>
    {
        public void Configure(EntityTypeBuilder<TbFulfillmentFee> entity)
        {
            // Table name
            entity.ToTable("TbFulfillmentFees");

            // Property configurations
            entity.Property(e => e.FeeType)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.BaseFee)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.PercentageFee)
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.MinimumFee)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.MaximumFee)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.WeightBasedFeePerKg)
                .HasColumnType("decimal(10,3)");

            entity.Property(e => e.VolumeBasedFeePerCubicMeter)
                .HasColumnType("decimal(10,3)");

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.EffectiveFrom)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.EffectiveTo)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(500);

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(500);

            // Relationships
            entity.HasOne(e => e.FulfillmentMethod)
                .WithMany(m => m.FulfillmentFees)
                .HasForeignKey(e => e.FulfillmentMethodId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.FulfillmentMethodId);

            entity.HasIndex(e => e.FeeType);

            entity.HasIndex(e => e.IsActive);

            entity.HasIndex(e => new { e.FulfillmentMethodId, e.FeeType });

            entity.HasIndex(e => new { e.EffectiveFrom, e.EffectiveTo });
        }
    }
}
