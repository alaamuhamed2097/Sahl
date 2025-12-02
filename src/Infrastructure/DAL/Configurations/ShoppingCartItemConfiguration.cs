using Domains.Entities.ECommerceSystem.Cart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ShoppingCartItemConfiguration : IEntityTypeConfiguration<TbShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<TbShoppingCartItem> builder)
        {
            builder.ToTable("TbShoppingCartItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)");

            // Indexes
            builder.HasIndex(e => e.ShoppingCartId);
            builder.HasIndex(e => e.ItemId);
            builder.HasIndex(e => e.OfferId);
        }
    }
}
