using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Shared.DTOs.Base;
using System.Text.Json.Serialization;

namespace Shared.DTOs.ECommerce.Order
{
    /// <summary>
    /// Complete Order DTO for full details in Dashboard and comprehensive views
    /// </summary>
    public class OrderDto : BaseDto
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

        // User Information
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ProfileImagePath { get; set; } = null!;

        // Sponsor Information
        public string? SponsorFirstName { get; set; }
        public string? SponsorLastName { get; set; }
        public string? SponsorUserName { get; set; }

        // Payment Gateway Information
        public string PaymentGatewayMethodTitleEn { get; set; } = null!;
        public string PaymentGatewayMethodTitleAr { get; set; } = null!;
        public PaymentGatewayMethod PaymentGatewayMethodType { get; set; }

        // Order Items
        public List<OrderDetailsDto> OrderDetails { get; set; }
    }
}
