using Domains.Entities.Merchandising.HomePageBlocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbBlockProduct
    /// </summary>
    public class BlockProductConfiguration : IEntityTypeConfiguration<TbBlockItem>
    {
        public void Configure(EntityTypeBuilder<TbBlockItem> entity)
        {
            // Table name
            entity.ToTable("TbBlockProducts");

            // Property configurations
            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0);

            // Relationships
            entity.HasOne(e => e.HomepageBlock)
                .WithMany(b => b.BlockItems)
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

            entity.HasIndex(e => new { e.HomepageBlockId, e.ItemId })
                .IsUnique();

            entity.HasIndex(e => new { e.HomepageBlockId, e.DisplayOrder });
        }
    }
}
