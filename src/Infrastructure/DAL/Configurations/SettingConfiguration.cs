using Domains.Entities.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbSetting
    /// </summary>
    public class SettingConfiguration : IEntityTypeConfiguration<TbSetting>
    {
        public void Configure(EntityTypeBuilder<TbSetting> entity)
        {
            // Property configurations
            entity.Property(e => e.WhatsAppNumber)
                 .IsRequired()
              .HasMaxLength(20);
        }
    }
}
