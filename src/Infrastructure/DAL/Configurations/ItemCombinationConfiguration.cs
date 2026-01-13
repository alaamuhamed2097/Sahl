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

            entity.HasIndex(e => e.ItemId).IsUnique(false);

            // Filtered unique index: only one default combination per item when not deleted
            entity.HasIndex(e => new { e.ItemId, e.IsDefault })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0 AND [IsDefault] = 1"); // SQL Server syntax

            // Relationship with Item
            entity.HasOne(ic => ic.Item)
                .WithMany(i => i.ItemCombinations)
                .HasForeignKey(ic => ic.ItemId)
                .HasConstraintName("FK_TbItemCombinations_TbItems_ItemId")
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
