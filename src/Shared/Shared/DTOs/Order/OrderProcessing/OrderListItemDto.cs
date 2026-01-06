using Common.Enumerations.Shipping;

namespace Shared.DTOs.Order.OrderProcessing
{
    public class OrderListItemDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = null!;
  //      public string VindorNameAr { get; set; }
  //      public string VindorNameEn { get; set; }
		//public string ItemImageUrl { get; set; } = null!;
  //      public string ItemName { get; set; }
  //      public string ItemType { get; set; }
        public int QuantityItem { get; set; }
		public ShipmentStatus ShipmentStatus { get; set; }
		public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string OrderStatus { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}