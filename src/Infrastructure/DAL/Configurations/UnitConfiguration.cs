using Domins.Entities.Unit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbUnit
    /// </summary>
    public class UnitConfiguration : IEntityTypeConfiguration<TbUnit>
    {
        public void Configure(EntityTypeBuilder<TbUnit> entity)
        {
            // Property configurations
            entity.Property(e => e.TitleAr)
               .IsRequired()
                 .HasMaxLength(100);

            entity.Property(e => e.TitleEn)
            .IsRequired()
                  .HasMaxLength(100);
        }
    }
}
