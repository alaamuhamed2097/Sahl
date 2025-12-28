using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.Loyalty;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbCustomerLoyalty
    /// </summary>
    public class CustomerItemViewConfiguration : IEntityTypeConfiguration<TbCustomerItemView>
    {
        public void Configure(EntityTypeBuilder<TbCustomerItemView> entity)
        {
            // Table name
            entity.ToTable("TbCustomerItemViews");
			// Primary Key
            entity.HasKey(e => e.Id);

			// Relationships
			entity.HasOne(e => e.Customer)
                .WithMany(c=>c.CustomerItemViews)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ItemCombination)
                .WithMany(t => t.CustomerItemViews)
                .HasForeignKey(e => e.ItemCombinationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.CustomerId)
                .IsUnique(false);
            entity.HasIndex(e => e.ItemCombinationId)
                .IsUnique(false);
        }
    }
}
