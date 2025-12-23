using Domains.Entities.Catalog.Item.ItemAttributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbCombinationAttribute
    /// </summary>
    public class CombinationAttributesConfiguration : IEntityTypeConfiguration<TbCombinationAttribute>
    {
        public void Configure(EntityTypeBuilder<TbCombinationAttribute> entity)
        {
            // Indexes
            entity.HasIndex(e => e.ItemCombinationId)
                .IsUnique(false);

            entity.HasIndex(e => e.AttributeValueId)
                .IsUnique(false);

            // Relationships
            entity.HasOne(ic => ic.ItemCombination)
                .WithMany(i => i.CombinationAttributes)
                .HasForeignKey(ic => ic.ItemCombinationId)
                .HasConstraintName("FK_TbItemCombination_TbCombinationAttributes_ItemCombinationId")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ic => ic.CombinationAttributeValue)
                .WithMany(i => i.CombinationAttributes)
                .HasForeignKey(ic => ic.AttributeValueId)
                .HasConstraintName("FK_TbCombinationAttributesValue_TbCombinationAttributes_AttributeValueId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
