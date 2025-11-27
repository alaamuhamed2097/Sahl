using Domains.Entities.Offer.Warranty;
using Domains.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.ApplicationContext.Configurations
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

            // Warranty -> City (many warranties may be linked to a city)
            builder.HasOne<TbCity>(w => w.City)
                   .WithMany()
                   .HasForeignKey(w => w.CityId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Configure indexes
            builder.HasIndex(w => w.CityId);
        }
    }
}
