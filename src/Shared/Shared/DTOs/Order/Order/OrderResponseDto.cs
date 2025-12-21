using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Order.Order
{
    /// <summary>
    /// API Response DTO ???? ?????? ????? ?????? ??? ??????? ?????
    /// API Response DTO for returning order data to customer without sensitive information
    /// </summary>
    public class OrderResponseDto : BaseDto
    {
        public string Number { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal ShippingAmount { get; set; } = 0m;
        public string Address { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? InvoiceId { get; set; }
        public OrderStatus CurrentState { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime? OrderDeliveryDate { get; set; }

        [JsonIgnore]
        public string CreatedDateLocalFormatted =>
         TimeZoneInfo.ConvertTimeFromUtc(CreatedDateUtc, TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo")).ToString("yyyy-MM-dd HH:mm tt");

        [JsonIgnore]
        public string OrderDeliveryDateLocalFormatted =>
         TimeZoneInfo.ConvertTimeFromUtc(OrderDeliveryDate ?? new(), TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo")).ToString("yyyy-MM-dd HH:mm tt");

        // Calculated property to check if refund is still valid (within 15 days)
        public bool IsWithinRefundPeriod => DateTime.UtcNow.Subtract(CreatedDateUtc).Days <= 15;

        // Limited User Information (non-sensitive)
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        // Sponsor Information
        public string? SponsorUserName { get; set; }

        // Payment Gateway Information
        public string PaymentGatewayMethodTitleEn { get; set; } = null!;
        public string PaymentGatewayMethodTitleAr { get; set; } = null!;
        public PaymentGatewayMethod PaymentGatewayMethodType { get; set; }

        // Order Items
        public List<OrderDetailsDto> OrderDetails { get; set; }
    }
}
