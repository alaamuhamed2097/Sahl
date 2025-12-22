using Domains.Entities.Catalog.Unit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbUnitConversion
    /// </summary>
    public class UnitConversionConfiguration : IEntityTypeConfiguration<TbUnitConversion>
    {
        public void Configure(EntityTypeBuilder<TbUnitConversion> entity)
        {
            // Property configurations
            entity.Property(e => e.ConversionFactor)
              .HasColumnType("decimal(18,6)")
              .IsRequired();

            // Relationships
            entity.HasOne(uc => uc.FromUnit)
               .WithMany()
               .HasForeignKey(uc => uc.FromUnitId)
               .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(uc => uc.ToUnit)
                .WithMany()
                .HasForeignKey(uc => uc.ToUnitId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
