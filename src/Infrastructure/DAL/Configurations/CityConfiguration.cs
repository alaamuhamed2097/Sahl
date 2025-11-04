using Domins.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<TbCity>
    {
        public void Configure(EntityTypeBuilder<TbCity> entity)
        {
            entity.Property(e => e.TitleEn)
                    .IsRequired()
                    .HasMaxLength(100);

            entity.Property(e => e.TitleAr)
                .IsRequired()
                .HasDefaultValue("")
                .HasMaxLength(100);

            entity.HasOne(d => d.State)
                .WithMany(s => s.Cities)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_TbCities_TbStates_StateId");

            entity.HasIndex(e => e.StateId).IsUnique(false);
        }
    }
}
