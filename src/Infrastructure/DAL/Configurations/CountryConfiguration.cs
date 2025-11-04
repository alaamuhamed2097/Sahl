using Domins.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<TbCountry>
    {
        public void Configure(EntityTypeBuilder<TbCountry> entity)
        {
            entity.Property(e => e.TitleEn)
                    .IsRequired()
                    .HasMaxLength(100);

            entity.Property(e => e.TitleAr)
                .IsRequired()
                .HasDefaultValue("")
                .HasMaxLength(100);
        }
    }
}
