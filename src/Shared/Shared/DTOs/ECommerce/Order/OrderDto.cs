using Resources;
using Resources.Enumerations;
using Shared.DTOs.Base;
using Common.Enumerations.Payment;
using System.Text.Json.Serialization;
using Common.Enumerations.Order;

namespace Shared.DTOs.ECommerce
{
    public class OrderDto : BaseDto
    {
        public string Number { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal ShippingAmount { get; set; } = 0m;
        public string Address { get; set; }
        public int PVs { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? InvoiceId { get; set; }
        public OrderStatus CurrentState { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime? OrderDeliveryDate { get; set; }
        public Guid? DirectSaleLinkId { get; set; }
        
        [JsonIgnore]
        public string CreatedDateLocalFormatted =>
         TimeZoneInfo.ConvertTimeFromUtc(CreatedDateUtc, TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo")).ToString("yyyy-MM-dd HH:mm tt");

        [JsonIgnore]
        public string OrderDeliveryDateLocalFormatted =>
         TimeZoneInfo.ConvertTimeFromUtc(OrderDeliveryDate ?? new(), TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo")).ToString("yyyy-MM-dd HH:mm tt");

        // Calculated property to check if refund is still valid (within 15 days)
        public bool IsWithinRefundPeriod => DateTime.UtcNow.Subtract(CreatedDateUtc).Days <= 15;

        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ProfileImagePath { get; set; } = null!;

        public string? SponsorFirstName { get; set; }
        public string? SponsorLastName { get; set; }
        public string? SponsorUserName { get; set; }

        public Guid PaymentGatewayMethodId { get; set; }
        public string PaymentGatewayMethodTitleEn { get; set; } = null!;
        public string PaymentGatewayMethodTitleAr { get; set; } = null!;
        public PaymentGatewayMethod PaymentGatewayMethodType { get; set; }
        public Guid? ShippingCompanyId { get; set; }

        [JsonIgnore]
        public string PaymentGatewayMethodTitle => ResourceManager.CurrentLanguage == Language.Arabic ? PaymentGatewayMethodTitleAr : PaymentGatewayMethodTitleEn;
        
        // Business Points Information
        /// <summary>
        /// Number of business points used for this order
        /// </summary>
        public int BusinessPointsUsed { get; set; }

        /// <summary>
        /// Dollar value of business points consumed
        /// </summary>
        public decimal BusinessPointsValue { get; set; }

        /// <summary>
        /// Total order price before business points were applied
        /// </summary>
        public decimal TotalBeforePoints { get; set; }

        [JsonIgnore]
        public bool UsedBusinessPoints => BusinessPointsUsed > 0;

        public List<OrderDetailsDto> OrderDetails { get; set; }
    }
}
