using Shared.DTOs.Order.OrderProcessing.AdminOrder;

namespace Shared.DTOs.Order.OrderProcessing.VendorOrder
{
    public class VendorGroupDto
    {
        public Guid VendorId { get; set; }
        public string VendorName { get; set; } = null!;
        public List<AdminOrderItemDto> Items { get; set; } = new();
        public decimal VendorSubTotal { get; set; }
    }
}
