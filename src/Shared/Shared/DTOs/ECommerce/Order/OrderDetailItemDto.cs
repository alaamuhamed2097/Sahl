namespace Shared.DTOs.ECommerce.Order
{
    /// <summary>
    /// Order detail item
    /// </summary>
    public class OrderDetailItemDto
    {
        public Guid OrderDetailId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; } = null!;
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; } = null!;
    }
}
