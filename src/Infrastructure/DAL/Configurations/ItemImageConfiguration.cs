using Domains.Entities.Catalog.Item;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbItemImage
    /// </summary>
    public class ItemImageConfiguration : IEntityTypeConfiguration<TbItemImage>
    {
        public void Configure(EntityTypeBuilder<TbItemImage> entity)
        {
            // Property configurations
            entity.Property(e => e.Path)
            .IsRequired()
             .HasMaxLength(200);

            // Indexes
            entity.HasIndex(e => e.Order)
       .IsUnique(false);

            // Relationships
            entity.HasOne(ii => ii.Item)
                .WithMany(i => i.ItemImages)
              .HasForeignKey(ii => ii.ItemId)
                     .HasConstraintName("FK_TbItemImages_TbItems_ItemId")
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
