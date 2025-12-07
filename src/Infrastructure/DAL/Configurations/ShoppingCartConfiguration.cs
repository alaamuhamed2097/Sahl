using Domains.Entities.ECommerceSystem.Cart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<TbShoppingCart>
    {
        public void Configure(EntityTypeBuilder<TbShoppingCart> builder)
        {
            builder.ToTable("TbShoppingCarts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(e => e.UserId);
        }
    }
}
