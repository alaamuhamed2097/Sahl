using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order.Cart
{
    public class TbShoppingCart : BaseEntity
    {
        [ForeignKey("User")]
        public string UserId { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;

        public virtual ICollection<TbShoppingCartItem> Items { get; set; } = new HashSet<TbShoppingCartItem>();
    }
}
