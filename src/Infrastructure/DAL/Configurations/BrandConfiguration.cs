using Domains.Entities.Catalog.Brand;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbBrand
    /// </summary>
    public class BrandConfiguration : IEntityTypeConfiguration<TbBrand>
    {
        public void Configure(EntityTypeBuilder<TbBrand> entity)
        {
            // Property configurations
            entity.Property(e => e.NameEn)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.NameAr)
              .IsRequired()
                   .HasMaxLength(50);

            entity.Property(e => e.LogoPath)
              .IsRequired()
              .HasMaxLength(200);

            entity.Property(e => e.WebsiteUrl)
            .HasMaxLength(200);


            entity.Property(e => e.DisplayOrder)
            .HasDefaultValue(0);


            entity.HasIndex(e => e.DisplayOrder)
               .IsUnique(false);
        }
    }
}
