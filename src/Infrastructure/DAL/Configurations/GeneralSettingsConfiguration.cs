using Domains.Entities.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbGeneralSettings
    /// </summary>
    public class GeneralSettingsConfiguration : IEntityTypeConfiguration<TbGeneralSettings>
    {
        public void Configure(EntityTypeBuilder<TbGeneralSettings> entity)
        {
            entity.ToTable("TbGeneralSettings");

            // Indexes for better performance
            entity.HasIndex(e => e.Email).IsUnique();

            // Property configurations
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(15);

            entity.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.FacebookUrl)
                .HasMaxLength(200);

            entity.Property(e => e.InstagramUrl)
                .HasMaxLength(200);

            entity.Property(e => e.TwitterUrl)
                .HasMaxLength(200);

            entity.Property(e => e.LinkedInUrl)
                .HasMaxLength(200);

            entity.Property(e => e.WhatsAppNumber)
                .HasMaxLength(15);
        }
    }
}