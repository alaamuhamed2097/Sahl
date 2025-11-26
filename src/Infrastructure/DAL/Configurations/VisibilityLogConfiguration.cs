using Domains.Entities.Visibility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbVisibilityLog
    /// </summary>
    public class VisibilityLogConfiguration : IEntityTypeConfiguration<TbVisibilityLog>
    {
        public void Configure(EntityTypeBuilder<TbVisibilityLog> entity)
        {
            // Table name
            entity.ToTable("TbVisibilityLogs");

            // Property configurations
            entity.Property(e => e.WasVisible)
                .IsRequired();

            entity.Property(e => e.IsVisible)
                .IsRequired();

            entity.Property(e => e.ChangedAt)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.ChangeReason)
                .HasMaxLength(500);

            entity.Property(e => e.IsAutomatic)
                .HasDefaultValue(false);

            // Relationships
            entity.HasOne(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ChangedByUser)
                .WithMany()
                .HasForeignKey(e => e.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.ItemId);

            entity.HasIndex(e => e.ChangedAt);

            entity.HasIndex(e => e.IsAutomatic);

            entity.HasIndex(e => new { e.ItemId, e.ChangedAt });
        }
    }
}
