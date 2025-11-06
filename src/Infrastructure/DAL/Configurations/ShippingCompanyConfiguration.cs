using Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbShippingCompany
    /// </summary>
    public class ShippingCompanyConfiguration : IEntityTypeConfiguration<TbShippingCompany>
    {
        public void Configure(EntityTypeBuilder<TbShippingCompany> entity)
        {
            // Property configurations
            entity.Property(s => s.LogoImagePath)
             .IsRequired()
                 .HasMaxLength(300);

            entity.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            entity.Property(e => e.PhoneCode)
                       .IsRequired()
                .HasMaxLength(4);

            entity.Property(e => e.PhoneNumber)
                         .IsRequired()
               .HasMaxLength(15);
        }
    }
}
