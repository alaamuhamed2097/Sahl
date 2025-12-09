using Domains.Entities.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class NotificationPreferencesConfiguration : IEntityTypeConfiguration<TbNotificationPreferences>
    {
        public void Configure(EntityTypeBuilder<TbNotificationPreferences> builder)
        {
            // Use the same table name used by existing migrations/snapshot
            builder.ToTable("TbNotificationPreferences");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.UserId)
                .IsRequired()
                // ApplicationUser uses string Id (nvarchar(450))
                .HasColumnType("nvarchar(450)");

            builder.Property(x => x.UserType)
                .IsRequired();

            builder.Property(x => x.NotificationType)
                .IsRequired();

            builder.Property(x => x.EnableEmail)
                .HasDefaultValue(true);

            builder.Property(x => x.EnableSMS)
                .HasDefaultValue(false);

            builder.Property(x => x.EnablePush)
                .HasDefaultValue(true);

            builder.Property(x => x.EnableInApp)
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedDateUtc)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.UpdatedDateUtc)
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(1);

            // Relationships: configure FK to ApplicationUser (string id)
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => x.IsDeleted);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new { x.UserId, x.UserType });
            builder.HasIndex(x => x.NotificationType);

            // Unique constraint
            builder.HasIndex(x => new { x.UserId, x.UserType, x.NotificationType })
                .IsUnique();
        }
    }
}
