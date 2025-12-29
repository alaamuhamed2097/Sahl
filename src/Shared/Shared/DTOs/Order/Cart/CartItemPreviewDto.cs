namespace Shared.DTOs.Order.Cart
{
    /// <summary>
    /// Cart item in checkout preview
    /// </summary>
    public class CartItemPreviewDto
    {
        public Guid CartItemId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public string VendorName { get; set; } = null!;
        public Guid VendorId { get; set; }
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; } = null!;
        public bool IsAvailable { get; set; }
    }
}
