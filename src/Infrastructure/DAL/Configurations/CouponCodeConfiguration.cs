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
            entity.Property(p => p.TitleAR)
                 .IsRequired()
                  .HasMaxLength(100);

            entity.Property(p => p.TitleEN)
            .IsRequired()
             .HasMaxLength(100);

            entity.Property(p => p.Code)
           .IsRequired()
             .HasMaxLength(100);

            entity.Property(p => p.Value)
                .IsRequired()
              .HasAnnotation("Range", new[] { 0.1, double.MaxValue });

            entity.Property(p => p.UsageLimit)
            .IsRequired(false)
          .HasAnnotation("Range", new[] { 1, int.MaxValue });

            entity.Property(p => p.UsageCount)
   .IsRequired()
     .HasAnnotation("Range", new[] { 0, int.MaxValue })
     .HasDefaultValue(0);

            entity.Property(p => p.StartDateUTC)
                .HasDefaultValueSql("GETUTCDATE()")
             .HasColumnType("datetime2(2)");

            entity.Property(p => p.EndDateUTC)
             .HasDefaultValueSql("GETUTCDATE()")
              .HasColumnType("datetime2(2)");

            // Indexes
            entity.HasIndex(e => e.Code)
               .IsUnique(true);
        }
    }
}
