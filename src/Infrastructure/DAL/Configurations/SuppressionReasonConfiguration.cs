using Domains.Entities.Visibility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbSuppressionReason
    /// </summary>
    public class SuppressionReasonConfiguration : IEntityTypeConfiguration<TbSuppressionReason>
    {
        public void Configure(EntityTypeBuilder<TbSuppressionReason> entity)
        {
            // Table name
            entity.ToTable("TbSuppressionReasons");

            // Property configurations
            entity.Property(e => e.ReasonType)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.ReasonDescriptionEn)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.ReasonDescriptionAr)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.DetectedAt)
                .IsRequired()
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.ResolvedAt)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.IsResolved)
                .HasDefaultValue(false);

            entity.Property(e => e.ResolutionNotes)
                .HasMaxLength(1000);

            // Relationships
            entity.HasOne(e => e.ProductVisibilityRule)
                .WithMany(r => r.SuppressionReasons)
                .HasForeignKey(e => e.ProductVisibilityRuleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.ProductVisibilityRuleId);

            entity.HasIndex(e => e.ReasonType);

            entity.HasIndex(e => e.IsResolved);

            entity.HasIndex(e => e.DetectedAt);

            entity.HasIndex(e => new { e.ProductVisibilityRuleId, e.ReasonType });

            entity.HasIndex(e => new { e.IsResolved, e.DetectedAt });
        }
    }
}
