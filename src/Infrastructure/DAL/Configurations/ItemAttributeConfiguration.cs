using Domains.Entities.Catalog.Item.ItemAttributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbItemAttribute
    /// </summary>
    //public class ItemAttributeConfiguration : IEntityTypeConfiguration<TbItemAttribute>
    //{
    //    public void Configure(EntityTypeBuilder<TbItemAttribute> entity)
    //    {
    //        // Property configurations
    //        entity.Property(e => e.Value)
    //           .IsRequired()
    //             .HasMaxLength(200);

    //        // Relationships
    //        entity.HasOne(ia => ia.Item)
    //                 .WithMany(i => i.ItemAttributes)
    //                    .HasForeignKey(ia => ia.ItemId)
    //                .HasConstraintName("FK_TbItemAttributes_TbItems_ItemId")
    //                 .OnDelete(DeleteBehavior.Cascade);

    //        entity.HasOne(ia => ia.Attribute)
    //           .WithMany(a => a.ItemAttributes)
    //      .HasForeignKey(ia => ia.AttributeId)
    //                .HasConstraintName("FK_TbItemAttributes_TbAttributes_AttributeId")
    //        .OnDelete(DeleteBehavior.Cascade);
    //    }
    //}
}
