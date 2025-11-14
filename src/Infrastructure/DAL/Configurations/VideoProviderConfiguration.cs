using Domains.Entities.VideoProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbVideoProvider
    /// </summary>
    public class VideoProviderConfiguration : IEntityTypeConfiguration<TbVideoProvider>
    {
        public void Configure(EntityTypeBuilder<TbVideoProvider> entity)
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
