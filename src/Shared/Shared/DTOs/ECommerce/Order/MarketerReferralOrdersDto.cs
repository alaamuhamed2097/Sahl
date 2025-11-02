using Common.Enumerations.Commission;
using Common.Enumerations.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.DTOs.ECommerce.Order
{
    public class MarketerReferralOrdersDto
    {
        // Marketer Info
        public Guid MarketerId { get; set; }

        // Direct Sale Link Info
        public string DirectSaleCode { get; set; } = null!;

        // Order Info
        public string OrderNumber { get; set; } = null!;

        public decimal OrderPrice { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public DateTime? PaymentDate { get; set; }

        // Customer Info
        public string CustomerName { get; set; } = null!;

        public string CustomerPhone { get; set; } = null!;

        // Commission Info
        public CommissionType? CommissionType { get; set; }

        public decimal? CommissionRate { get; set; }

        public decimal? CommissionAmount { get; set; }

        [JsonIgnore]
        public string PaymentDateLocalFormatted =>
         TimeZoneInfo.ConvertTimeFromUtc(PaymentDate ?? new DateTime(), TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo")).ToString("yyyy-MM-dd HH:mm tt");
    }
}
