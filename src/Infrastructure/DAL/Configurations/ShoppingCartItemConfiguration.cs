using Domains.Entities.Order.Cart;
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

            // Foreign Keys
            builder.HasOne(x => x.ShoppingCart)
                .WithMany(c => c.Items)
                .HasForeignKey(x => x.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Item)
                .WithMany()
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Foreign key to OfferCombinationPricing (OfferId column stores the OfferCombinationPricingId)
            builder.HasOne(x => x.OfferCombinationPricing)
                .WithMany()
                .HasForeignKey(x => x.OfferCombinationPricingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(e => e.ShoppingCartId);
            builder.HasIndex(e => e.ItemId);
            builder.HasIndex(e => e.OfferCombinationPricingId);
            builder.HasIndex(e => new { e.ShoppingCartId, e.ItemId, e.OfferCombinationPricingId });
        }
    }
}


