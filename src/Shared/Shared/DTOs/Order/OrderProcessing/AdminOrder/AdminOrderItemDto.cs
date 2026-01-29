using Common.Enumerations.Shipping;
using Resources;
using Resources.Enumerations;

namespace Shared.DTOs.Order.OrderProcessing.AdminOrder
{
    public class AdminOrderItemDto
    {
        public Guid OrderDetailId { get; set; }
        public Guid ItemId { get; set; }
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string Title => ResourceManager.CurrentLanguage == Language.Arabic ? TitleAr : TitleEn;
        public string ItemImage { get; set; } = null!;
        public Guid VendorId { get; set; }
        public string VendorName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public Guid? WarehouseId { get; set; }
    }
}
