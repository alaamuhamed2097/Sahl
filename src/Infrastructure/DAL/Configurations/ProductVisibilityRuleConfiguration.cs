using Domains.Entities.Visibility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbProductVisibilityRule
    /// </summary>
    public class ProductVisibilityRuleConfiguration : IEntityTypeConfiguration<TbProductVisibilityRule>
    {
        public void Configure(EntityTypeBuilder<TbProductVisibilityRule> entity)
        {
            // Table name
            entity.ToTable("TbProductVisibilityRules");

            // Property configurations
            entity.Property(e => e.VisibilityStatus)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.HasActiveOffers)
                .HasDefaultValue(false);

            entity.Property(e => e.HasStock)
                .HasDefaultValue(false);

            entity.Property(e => e.IsApproved)
                .HasDefaultValue(false);

            entity.Property(e => e.SuppressedAt)
                .HasColumnType("datetime2(2)");

            // Relationships
            entity.HasOne(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.SuppressedByUser)
                .WithMany()
                .HasForeignKey(e => e.SuppressedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.ItemId)
                .IsUnique();

            entity.HasIndex(e => e.VisibilityStatus);
        }
    }
}
