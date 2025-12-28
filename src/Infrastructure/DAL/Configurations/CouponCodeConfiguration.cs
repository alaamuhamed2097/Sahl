using Domains.Entities.CouponCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbCouponCode
    /// </summary>
    public class CouponCodeConfiguration : IEntityTypeConfiguration<TbCouponCode>
    {
        public void Configure(EntityTypeBuilder<TbCouponCode> entity)
        {
            // Property configurations
            entity.Property(p => p.TitleAr)
                 .IsRequired()
                  .HasMaxLength(100);

            entity.Property(p => p.TitleEn)
            .IsRequired()
             .HasMaxLength(100);

            entity.Property(p => p.Code)
           .IsRequired()
             .HasMaxLength(100);

            entity.Property(p => p.DiscountValue)
                .IsRequired()
              .HasAnnotation("Range", new[] { 0.1, double.MaxValue });

            entity.Property(p => p.UsageLimit)
            .IsRequired(false)
          .HasAnnotation("Range", new[] { 1, int.MaxValue });

            entity.Property(p => p.UsageCount)
   .IsRequired()
     .HasAnnotation("Range", new[] { 0, int.MaxValue })
     .HasDefaultValue(0);

            entity.Property(p => p.StartDate)
                .HasDefaultValueSql("GETUTCDATE()")
             .HasColumnType("datetime2(2)");

            entity.Property(p => p.ExpiryDate)
             .HasDefaultValueSql("GETUTCDATE()")
              .HasColumnType("datetime2(2)");

            // Indexes
            entity.HasIndex(e => e.Code)
               .IsUnique(true);
        }
    }
}
