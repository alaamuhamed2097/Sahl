namespace Shared.DTOs.ECommerce.Checkout
{
    public class CheckoutSummaryDto
    {
        public Guid OrderId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal EstimatedShipping { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public List<ExpectedShipmentDto> ExpectedShipments { get; set; } = new();
        public int ShipmentCount { get; set; }
    }

    public class ExpectedShipmentDto
    {
        public string ShipmentNumber { get; set; } = null!;
        public string VendorName { get; set; } = null!;
        public string WarehouseName { get; set; } = null!;
        public List<ShipmentItemPreviewDto> Items { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal EstimatedShipping { get; set; }
        public decimal Total { get; set; }
    }

    public class ShipmentItemPreviewDto
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class PrepareCheckoutRequest
    {
        public Guid DeliveryAddressId { get; set; }
    }
}
