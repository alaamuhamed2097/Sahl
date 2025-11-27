using Domains.Entities.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class NotificationPreferencesConfiguration : IEntityTypeConfiguration<TbNotificationPreferences>
    {
        public void Configure(EntityTypeBuilder<TbNotificationPreferences> builder)
        {
            builder.ToTable("NotificationPreferences");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasColumnType("uniqueidentifier");

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

            builder.Property(x => x.CurrentState)
                .HasDefaultValue(1);

            // Relationships: configure FK to ApplicationUser (Guid)
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => x.CurrentState);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new { x.UserId, x.UserType });
            builder.HasIndex(x => x.NotificationType);

            // Unique constraint
            builder.HasIndex(x => new { x.UserId, x.UserType, x.NotificationType })
                .IsUnique();
        }
    }
}
