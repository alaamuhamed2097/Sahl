using Domains.Entities.Merchandising.CouponCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbCouponCodeScope
    /// </summary>
    public class CouponCodeScopeConfiguration : IEntityTypeConfiguration<TbCouponCodeScope>
    {
        public void Configure(EntityTypeBuilder<TbCouponCodeScope> entity)
        {
            entity.ToTable("TbCouponCodeScopes");

            // Primary Key
            entity.HasKey(e => e.Id);

            // Properties
            entity.Property(e => e.CouponCodeId)
                .IsRequired();

            entity.Property(e => e.ScopeType)
                .IsRequired();

            entity.Property(e => e.ScopeId)
                .IsRequired();

            // Relationships
            entity.HasOne(e => e.CouponCode)
                .WithMany(c => c.CouponScopes)
                .HasForeignKey(e => e.CouponCodeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => new { e.CouponCodeId, e.ScopeType, e.ScopeId })
                .IsUnique()
                .HasDatabaseName("IX_CouponCodeScope_CouponId_Type_ScopeId");

            entity.HasIndex(e => e.CouponCodeId)
                .HasDatabaseName("IX_CouponCodeScope_CouponCodeId");
        }
    }
}
