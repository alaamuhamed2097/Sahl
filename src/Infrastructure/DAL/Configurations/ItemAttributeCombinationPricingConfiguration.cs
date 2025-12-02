//using Domains.Entities.Catalog.Item.ItemAttributes;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace DAL.Configurations
//{
//    /// <summary>
//    /// Entity configuration for TbItemAttributeCombinationPricing
//    /// </summary>
//    public class ItemAttributeCombinationPricingConfiguration : IEntityTypeConfiguration<TbItemAttributeCombinationPricing>
//    {
//        public void Configure(EntityTypeBuilder<TbItemAttributeCombinationPricing> entity)
//        {
//            // Property configurations
//            entity.Property(e => e.AttributeIds)
//                .IsRequired()
//                .HasMaxLength(500);

//            entity.Property(e => e.Price)
//                .IsRequired()
//                .HasColumnType("decimal(18,2)");

//            entity.Property(e => e.SalesPrice)
//                .IsRequired()
//                .HasColumnType("decimal(18,2)");

//            entity.Property(e => e.Quantity)
//                .IsRequired();

//            // Indexes
//            entity.HasIndex(e => e.AttributeIds)
//                .IsUnique(false);

//            entity.HasIndex(e => e.Price)
//                .IsUnique(false);
//        }
//    }
//}
