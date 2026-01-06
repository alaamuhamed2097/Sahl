using Domains.Entities.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbSystemSettings
    /// </summary>
    public class SystemSettingsConfiguration : IEntityTypeConfiguration<TbSystemSettings>
    {
        public void Configure(EntityTypeBuilder<TbSystemSettings> entity)
        {
            entity.ToTable("TbSystemSettings");

            // Composite index for better query performance
            entity.HasIndex(e => new { e.Category, e.SettingKey }).IsUnique();

            // Property configurations
            entity.Property(e => e.SettingKey)
                .IsRequired();

            entity.Property(e => e.SettingValue)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.DataType)
                .IsRequired();

            entity.Property(e => e.Category)
                .IsRequired();
        }
    }
}