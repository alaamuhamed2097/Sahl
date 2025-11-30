using Domains.Entities.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class NotificationsConfiguration : IEntityTypeConfiguration<TbNotification>
    {
        public void Configure(EntityTypeBuilder<TbNotification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.RecipientID)
                .IsRequired();

            builder.Property(x => x.RecipientType)
                .IsRequired();

            builder.Property(x => x.NotificationType)
                .IsRequired();

            builder.Property(x => x.Severity)
                .IsRequired()
                .HasDefaultValue(Common.Enumerations.Notification.Severity.Medium);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Message)
                .IsRequired();

            builder.Property(x => x.Data)
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.IsRead)
                .HasDefaultValue(false);

            builder.Property(x => x.ReadDate)
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.SentDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.DeliveryStatus)
                .IsRequired()
                .HasDefaultValue(Common.Enumerations.Notification.DeliveryStatus.Pending);

            builder.Property(x => x.DeliveryChannel)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedDateUtc)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.UpdatedDateUtc)
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.CurrentState)
                .HasDefaultValue(1);

            // Indexes
            builder.HasIndex(x => x.CurrentState);
            builder.HasIndex(x => new { x.RecipientType, x.RecipientID });
            builder.HasIndex(x => x.IsRead);
            builder.HasIndex(x => x.SentDate);
            builder.HasIndex(x => x.NotificationType);
        }
    }
}
