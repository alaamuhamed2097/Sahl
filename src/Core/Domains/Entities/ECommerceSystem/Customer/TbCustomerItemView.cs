using Domains.Entities.Catalog.Item.ItemAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domains.Entities.ECommerceSystem.Customer
{
    public class TbCustomerItemView : BaseEntity
    {
        [Required]
        public Guid ItemCombinationId { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
        // Navigation properties
        [ForeignKey("ItemCombinationId")]
        public virtual TbItemCombination ItemCombination { get; set; }
        [ForeignKey("CustomerId")]
        public virtual TbCustomer Customer { get; set; }
    }
}
