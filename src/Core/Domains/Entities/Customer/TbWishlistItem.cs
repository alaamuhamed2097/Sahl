using Domains.Entities.Catalog.Item.ItemAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Customer
{
    /// <summary>
    /// Represents an individual item in a customer's wishlist
    /// Links item combinations to wishlists
    /// </summary>
    [Table("TbWishlistItem")]
    public class TbWishlistItem : BaseEntity
    {
        /// <summary>
        /// Reference to the parent wishlist
        /// </summary>
        [Required]
        [Column("WishlistId")]
        [ForeignKey("Wishlist")]
        public Guid WishlistId { get; set; }

        /// <summary>
        /// The item combination that was added to wishlist
        /// This represents a specific variant/combination of a product
        /// </summary>
        [Required]
        [Column("ItemCombinationId")]
        [ForeignKey("ItemCombination")]
        public Guid ItemCombinationId { get; set; }

        /// <summary>
        /// Date when the item was added to wishlist
        /// </summary>
        [Required]
        [Column("DateAdded")]
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual TbWishlist Wishlist { get; set; } = null!;
        public virtual TbItemCombination ItemCombination { get; set; } = null!;
    }
}
