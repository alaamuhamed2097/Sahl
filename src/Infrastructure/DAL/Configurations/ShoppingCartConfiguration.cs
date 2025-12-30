using Domains.Entities.Order.Cart;
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

            // Indexes
            builder.HasIndex(e => e.UserId);
        }
    }
}
