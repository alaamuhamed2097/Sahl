using Domains.Entities.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class NotificationChannelConfiguration : IEntityTypeConfiguration<TbNotificationChannel>
    {
        public void Configure(EntityTypeBuilder<TbNotificationChannel> builder)
        {
            builder.ToTable("TbNotificationChannels");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.TitleAr)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.TitleEn)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Channel)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.Configuration)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedDateUtc)
                .HasDefaultValueSql("GETUTCDATE()")
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.UpdatedDateUtc)
                .HasColumnType("datetime2(2)");

            builder.Property(x => x.CurrentState)
                .HasDefaultValue(1);

            builder.HasIndex(x => x.CurrentState);
            builder.HasIndex(x => x.Channel);
            builder.HasIndex(x => x.IsActive);
        }
    }
}
