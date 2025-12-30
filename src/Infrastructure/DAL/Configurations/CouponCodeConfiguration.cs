using Domains.Entities.Merchandising.CouponCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbCouponCode - CORRECTED VERSION
    /// </summary>
    public class CouponCodeConfiguration : IEntityTypeConfiguration<TbCouponCode>
    {
        public void Configure(EntityTypeBuilder<TbCouponCode> entity)
        {
            // Table name
            entity.ToTable("TbCouponCodes");

            // Primary key
            entity.HasKey(e => e.Id);

            // Properties
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.TitleAr)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.TitleEn)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.PromoType)
                .IsRequired();

            entity.Property(e => e.DiscountType)
                .IsRequired();

            entity.Property(e => e.DiscountValue)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.MaxDiscountAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.MinimumOrderAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.MinimumProductPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.PlatformSharePercentage)
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.UsageCount)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(e => e.IsFirstOrderOnly)
                .IsRequired()
                .HasDefaultValue(false);

            // Relationships
            entity.HasOne(e => e.Vendor)
                .WithMany()
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.CouponScopes)
                .WithOne(s => s.CouponCode)
                .HasForeignKey(s => s.CouponCodeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Orders)
                .WithOne()
                .HasForeignKey(o => o.CouponId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.Code)
                .IsUnique()
                .HasDatabaseName("IX_TbCouponCodes_Code");

            entity.HasIndex(e => e.VendorId)
                .HasDatabaseName("IX_TbCouponCodes_VendorId");

            entity.HasIndex(e => new { e.IsActive, e.StartDate, e.ExpiryDate })
                .HasDatabaseName("IX_TbCouponCodes_Active_Dates");

            entity.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_TbCouponCodes_IsDeleted");

            // Query filters for soft delete
            entity.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}