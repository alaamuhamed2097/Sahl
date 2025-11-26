using Domains.Entities.Merchandising;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbBlockProduct
    /// </summary>
    public class BlockProductConfiguration : IEntityTypeConfiguration<TbBlockProduct>
    {
        public void Configure(EntityTypeBuilder<TbBlockProduct> entity)
        {
            // Table name
            entity.ToTable("TbBlockProducts");

            // Property configurations
            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.IsFeatured)
                .HasDefaultValue(false);

            entity.Property(e => e.BadgeText)
                .HasMaxLength(100);

            entity.Property(e => e.BadgeColor)
                .HasMaxLength(50);

            entity.Property(e => e.FeaturedFrom)
                .HasColumnType("datetime2(2)");

            entity.Property(e => e.FeaturedTo)
                .HasColumnType("datetime2(2)");

            // Relationships
            entity.HasOne(e => e.HomepageBlock)
                .WithMany(b => b.BlockProducts)
                .HasForeignKey(e => e.HomepageBlockId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.HomepageBlockId);

            entity.HasIndex(e => e.ItemId);

            entity.HasIndex(e => e.DisplayOrder);

            entity.HasIndex(e => e.IsActive);

            entity.HasIndex(e => e.IsFeatured);

            entity.HasIndex(e => new { e.HomepageBlockId, e.ItemId })
                .IsUnique();

            entity.HasIndex(e => new { e.HomepageBlockId, e.DisplayOrder });
        }
    }
}
