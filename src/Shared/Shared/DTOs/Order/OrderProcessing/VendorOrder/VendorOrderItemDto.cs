using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Common.Enumerations.Shipping;
using Shared.DTOs.Base;
using Shared.DTOs.Order.Checkout.Address;
using Shared.DTOs.Order.CouponCode;
using Shared.DTOs.Order.Fulfillment.Shipment;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Order.OrderProcessing.VendorDashboardOrder
{
    public class VendorOrderItemDto
    {
        public Guid OrderDetailId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public string ItemImage { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public Guid? WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
    }
}
