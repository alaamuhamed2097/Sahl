using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Customer
{
    /// <summary>
    /// Represents a customer's wishlist
    /// Each customer has one wishlist
    /// </summary>
    [Table("TbWishlist")]
    public class TbWishlist : BaseEntity
    {
        /// <summary>
        /// Customer ID who owns this wishlist
        /// </summary>
        [Required]
        [MaxLength(450)]
        [Column("CustomerId")]
        [ForeignKey("Customer")]
        public string CustomerId { get; set; } = null!;

        /// <summary>
        /// Date when wishlist was created
        /// </summary>
        [Required]
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ApplicationUser Customer { get; set; } = null!;
        public virtual ICollection<TbWishlistItem> WishlistItems { get; set; } = new List<TbWishlistItem>();
    }
}
