using Domains.Entities.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Entity configuration for TbWishlist
    /// One wishlist per customer
    /// </summary>
    public class WishlistConfiguration : IEntityTypeConfiguration<TbWishlist>
    {
        public void Configure(EntityTypeBuilder<TbWishlist> entity)
        {
            // Table Configuration
            entity.ToTable("TbWishlist");
            entity.HasKey(e => e.Id);

            // Column Mappings
            entity.Property(e => e.Id)
                .HasColumnName("WishlistId")
                .IsRequired()
                .ValueGeneratedNever();

            entity.Property(e => e.CustomerId)
                .HasColumnName("CustomerId")
                .IsRequired()
                .HasMaxLength(450);

            entity.Property(e => e.CreatedDate)
                .HasColumnName("CreatedDate")
                .IsRequired()
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(e => e.CreatedDateUtc)
                .IsRequired()
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.CreatedBy)
                .IsRequired();

            entity.Property(e => e.UpdatedDateUtc)
                .HasColumnType("datetime2");

            // Relationships
            entity.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.WishlistItems)
                .WithOne(wi => wi.Wishlist)
                .HasForeignKey(wi => wi.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            // Index for finding customer's wishlist
            entity.HasIndex(e => new { e.CustomerId, e.IsDeleted })
                .HasDatabaseName("IX_Wishlist_CustomerId_IsDeleted")
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            // Global Query Filter
            entity.HasQueryFilter(e => !e.IsDeleted);
        }
    }

    /// <summary>
    /// Entity configuration for TbWishlistItem
    /// Items in wishlists with unique constraint
    /// </summary>
    public class WishlistItemConfiguration : IEntityTypeConfiguration<TbWishlistItem>
    {
        public void Configure(EntityTypeBuilder<TbWishlistItem> entity)
        {
            // Table Configuration
            entity.ToTable("TbWishlistItem");
            entity.HasKey(e => e.Id);

            // Column Mappings
            entity.Property(e => e.Id)
                .HasColumnName("WishlistItemId")
                .IsRequired()
                .ValueGeneratedNever();

            entity.Property(e => e.WishlistId)
                .HasColumnName("WishlistId")
                .IsRequired();

            entity.Property(e => e.ItemCombinationId)
                .HasColumnName("ItemCombinationId")
                .IsRequired();

            entity.Property(e => e.DateAdded)
                .HasColumnName("DateAdded")
                .IsRequired()
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(e => e.CreatedDateUtc)
                .IsRequired()
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(e => e.CreatedBy)
                .IsRequired();

            entity.Property(e => e.UpdatedDateUtc)
                .HasColumnType("datetime2");

            // Relationships
            entity.HasOne(e => e.Wishlist)
                .WithMany(w => w.WishlistItems)
                .HasForeignKey(e => e.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ItemCombination)
                .WithMany()
                .HasForeignKey(e => e.ItemCombinationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            // Index for finding items in a wishlist
            entity.HasIndex(e => new { e.WishlistId, e.IsDeleted })
                .HasDatabaseName("IX_WishlistItem_WishlistId_IsDeleted")
                .HasFilter("[IsDeleted] = 0");

            // Index for checking if combination is wishlisted
            entity.HasIndex(e => new { e.ItemCombinationId, e.IsDeleted })
                .HasDatabaseName("IX_WishlistItem_ItemCombinationId_IsDeleted")
                .HasFilter("[IsDeleted] = 0");

            // UNIQUE CONSTRAINT: Customer cannot add same product (combination) twice
            entity.HasIndex(e => new { e.WishlistId, e.ItemCombinationId })
                .HasDatabaseName("UQ_WishlistItem_WishlistId_ItemCombinationId")
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            // Index for sorting by date added
            entity.HasIndex(e => e.DateAdded)
                .HasDatabaseName("IX_WishlistItem_DateAdded");

            // Global Query Filter
            entity.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
