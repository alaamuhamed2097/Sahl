using Domains.Entities.SellerTier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbSellerTierBenefit
    /// </summary>
    public class SellerTierBenefitConfiguration : IEntityTypeConfiguration<TbSellerTierBenefit>
    {
        public void Configure(EntityTypeBuilder<TbSellerTierBenefit> entity)
        {
            // Table name
            entity.ToTable("TbSellerTierBenefits");

            // Property configurations
            entity.Property(e => e.BenefitNameEn)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.BenefitNameAr)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(500);

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(500);

            entity.Property(e => e.IconPath)
                .HasMaxLength(200);

            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            // Relationships
            entity.HasOne(e => e.SellerTier)
                .WithMany(t => t.Benefits)
                .HasForeignKey(e => e.SellerTierId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.SellerTierId);

            entity.HasIndex(e => e.DisplayOrder);

            entity.HasIndex(e => e.IsActive);
        }
    }
}
