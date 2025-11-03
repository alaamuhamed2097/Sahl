using Domains.Entities.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbUserNotification
    /// </summary>
    public class UserNotificationConfiguration : IEntityTypeConfiguration<TbUserNotification>
    {
        public void Configure(EntityTypeBuilder<TbUserNotification> entity)
        {
            // Property configurations
            entity.Property(e => e.UserId)
            .IsRequired();

            entity.Property(e => e.NotificationId)
            .IsRequired();

            entity.Property(e => e.IsRead)
            .IsRequired()
                .HasDefaultValue(false);

            // Indexes
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.IsRead);
            entity.HasIndex(e => e.NotificationId);

            // Relationships
            entity.HasOne(e => e.User)
       .WithMany()
       .HasForeignKey(e => e.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.TbNotification)
               .WithMany(e => e.TbUserNotification)
               .HasForeignKey(e => e.NotificationId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
