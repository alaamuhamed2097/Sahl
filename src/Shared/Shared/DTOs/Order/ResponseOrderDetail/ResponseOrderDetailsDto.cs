using Common.Enumerations.Shipping;
using Shared.DTOs.Merchandising.CouponCode;
using Shared.DTOs.Order.Fulfillment.Shipment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Order.ResponseOrderDetail
{
	public class ResponseOrderDetailsDto
	{
		public Guid OrderDetailId { get; set; }
		public Guid ItemId { get; set; }
		public string ItemName { get; set; } = null!;
		public string ItemImageUrl { get; set; } = null!;
		//public string ItemType { get; set; }

		// Vendor
		public Guid VendorId { get; set; }
		public string VendorNameAr { get; set; }
		public string VendorNameEn { get; set; }

		// Pricing
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal SubTotal { get; set; }
		public decimal DiscountAmount { get; set; }
		public decimal TaxAmount { get; set; }

		// Status
		public ShipmentStatus ShipmentStatus { get; set; }
		//public string ShipmentStatusAr { get; set; } = null!;
		//public string ShipmentStatusEn { get; set; } = null!;
		//public string ShipmentStatusIcon { get; set; } = null!; 
	}

	public class ResponseOrderItemDetailDto
	{
		public Guid OrderDetailId { get; set; }
		public Guid ItemId { get; set; }
		//public string ItemName { get; set; } = null!;
		//public string ItemImageUrl { get; set; } = null!;
		//public string ItemType { get; set; }

		// Vendor
		public Guid VendorId { get; set; }
		public string VendorNameAr { get; set; }
		public string VendorNameEn { get; set; }

		// Pricing
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal SubTotal { get; set; }
		public decimal DiscountAmount { get; set; }
		public decimal TaxAmount { get; set; }

		// Status
		// ✅ الشحن
		public ShipmentStatus ShipmentStatus { get; set; }
		//public string ShipmentStatusAr { get; set; } = "";
		//public string ShipmentStatusEn { get; set; } = "";
		//public string ShipmentStatusIcon { get; set; } = "";
	}

	public class ResponseOrderAddressDto
	{
		public Guid AddressId { get; set; }
		public string FullAddress { get; set; } = null!;
		public string CityAr { get; set; }
		public string CityEn { get; set; }

		public string? PhoneNumber { get; set; }
	}

	public class ResponseShipmentTrackingDto
	{
		public Guid ShipmentId { get; set; }
		public string ShipmentNumber { get; set; }
		public ShipmentStatus CurrentStatus { get; set; }
		public List<ResponseShipmentStatusHistoryDto> StatusHistory { get; set; } = new();
	}

	public class ResponseShipmentStatusHistoryDto
	{
		public ShipmentStatus Status { get; set; }
		//public string StatusNameAr { get; set; } = null!;
		//public string StatusNameEn { get; set; } = null!;
		public DateTime? StatusDate { get; set; }
		public bool IsCompleted { get; set; }
		//public string Icon { get; set; } = null!; 
	}

	public class ResponseOrderItemDetailsDto
	{
		public Guid OrderDetailId { get; set; }
		public Guid ItemId { get; set; }
		public string ItemName { get; set; } = null!;
		public string ItemImageUrl { get; set; } = null!;
		public string ItemType { get; set; }

		// Vendor
		public Guid VendorId { get; set; }
		public string VendorNameAr { get; set; }
		public string VendorNameEn { get; set; }

		// Pricing
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal SubTotal { get; set; }

		// Status
		public ShipmentStatus ShipmentStatus { get; set; }
	}


}
