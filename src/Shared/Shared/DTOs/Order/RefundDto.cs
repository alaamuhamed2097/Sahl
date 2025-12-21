using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Shared.DTOs.Base;
using System.Text.Json.Serialization;

namespace Shared.DTOs.Order
{
    public class RefundDto : BaseDto
    {
        public decimal RefundAmount { get; set; }
        public string UserName { get; set; }
        public string RefundReason { get; set; } = string.Empty;
        public DateTime CreatedDateUtc { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public string CreatedDateLocalFormatted =>
            TimeZoneInfo.ConvertTimeFromUtc(CreatedDateUtc, TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo")).ToString("yyyy-MM-dd HH:mm tt");

        public RefundStatus CurrentState { get; set; } = RefundStatus.Pending;
        public string? AdminComments { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public PaymentGatewayMethod? RefundMethod { get; set; }
        public string? AdminUserId { get; set; }
        public Guid OrderId { get; set; } = Guid.Empty;
    }
}
