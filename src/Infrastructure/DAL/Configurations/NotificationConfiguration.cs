using Domains.Entities.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbNotification
    /// </summary>
    public class NotificationConfiguration : IEntityTypeConfiguration<TbNotification>
    {
        public void Configure(EntityTypeBuilder<TbNotification> entity)
        {
            // Property configurations
            entity.Property(e => e.TitleAr)
              .IsRequired()
             .HasMaxLength(200);

            entity.Property(e => e.TitleEn)
               .IsRequired()
          .HasMaxLength(200);

            entity.Property(e => e.DescriptionAr)
               .IsRequired()
               .HasMaxLength(1000);

            entity.Property(e => e.DescriptionEn)
               .IsRequired()
            .HasMaxLength(1000);
        }
    }
}
