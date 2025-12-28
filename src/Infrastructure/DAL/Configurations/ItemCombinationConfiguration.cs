using Domains.Entities.Catalog.Item.ItemAttributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbItemCombination
    /// </summary>
    public class ItemCombinationConfiguration : IEntityTypeConfiguration<TbItemCombination>
    {
        public void Configure(EntityTypeBuilder<TbItemCombination> entity)
        {
            entity.ToTable("TbItemCombinations");

			entity.HasKey(e => e.Id);

			entity.Property(e => e.Barcode)
                .HasMaxLength(200);

            entity.Property(e => e.SKU)
                .HasMaxLength(200);

            entity.HasIndex(e => e.ItemId).IsUnique(false);

            // Unique indexes for SKU and Barcode across table
            entity.HasIndex(e => e.SKU).IsUnique(true);
            entity.HasIndex(e => e.Barcode).IsUnique(true);

            // Relationship with Item
            entity.HasOne(ic => ic.Item)
                .WithMany(i => i.ItemCombinations)
                .HasForeignKey(ic => ic.ItemId)
                .HasConstraintName("FK_TbItemCombinations_TbItems_ItemId")
                .OnDelete(DeleteBehavior.Cascade);
		}
    }
}
