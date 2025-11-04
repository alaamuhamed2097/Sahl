using Domins.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class StateConfiguration : IEntityTypeConfiguration<TbState>
    {
        public void Configure(EntityTypeBuilder<TbState> entity)
        {
            entity.Property(e => e.TitleEn)
                    .IsRequired()
                    .HasMaxLength(100);

            entity.Property(e => e.TitleAr)
                .IsRequired()
                .HasMaxLength(100);

            // Relationships
            entity.HasOne(s => s.Country)
                .WithMany(c => c.States)
                .HasForeignKey(s => s.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => e.CountryId).IsUnique(false);
        }
    }
}
