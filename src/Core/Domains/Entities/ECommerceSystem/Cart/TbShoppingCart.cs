using Domains.Entities.ECommerceSystem.Customer;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.ECommerceSystem.Cart
{
    public class TbShoppingCart : BaseEntity
    {
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? ExpiresAt { get; set; }

        public virtual TbCustomer Customer { get; set; } = null!;

        public virtual ICollection<TbShoppingCartItem> Items { get; set; } = new HashSet<TbShoppingCartItem>();
    }
}
