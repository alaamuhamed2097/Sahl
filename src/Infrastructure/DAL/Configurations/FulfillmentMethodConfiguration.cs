using Domains.Entities.Fulfillment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbFulfillmentMethod
    /// </summary>
    public class FulfillmentMethodConfiguration : IEntityTypeConfiguration<TbFulfillmentMethod>
    {
        public void Configure(EntityTypeBuilder<TbFulfillmentMethod> entity)
        {
            // Table name
            entity.ToTable("TbFulfillmentMethods");

            // Property configurations
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.NameEn)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.NameAr)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(500);

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(500);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.BuyBoxPriorityBoost)
                .HasDefaultValue(0);

            entity.Property(e => e.RequiresWarehouse)
                .HasDefaultValue(false);

            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0);

            // Indexes
            entity.HasIndex(e => e.Code)
                .IsUnique();

            entity.HasIndex(e => e.IsActive);

            entity.HasIndex(e => e.DisplayOrder);
        }
    }
}
