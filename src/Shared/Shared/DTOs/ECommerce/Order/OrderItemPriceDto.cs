using Shared.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ECommerce.Order
{
    public class OrderItemPriceDto : BaseDto
    {
        public Guid ItemId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
