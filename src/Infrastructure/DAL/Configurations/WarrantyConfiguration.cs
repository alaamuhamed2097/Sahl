using Domains.Entities.Offer.Warranty;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class WarrantyConfiguration : IEntityTypeConfiguration<TbWarranty>
    {
        public void Configure(EntityTypeBuilder<TbWarranty> builder)
        {
            builder.ToTable("TbWarranties");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.WarrantyType).IsRequired();
            builder.Property(w => w.WarrantyPeriodMonths).IsRequired();
            builder.Property(w => w.WarrantyPolicy).HasMaxLength(500);
            builder.Property(w => w.WarrantyServiceCenter).HasMaxLength(500);
        }
    }
}
