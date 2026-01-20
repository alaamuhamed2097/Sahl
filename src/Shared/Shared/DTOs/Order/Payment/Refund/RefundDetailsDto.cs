using Common.Enumerations.Order;

namespace Shared.DTOs.Order.Payment.Refund
{
    public class RefundDetailsDto
    {
        // Offer basic fields
        public Guid Id { get; set; }

        // Vendor References
        public Guid VendorId { get; set; }
        public string VendorStoreName { get; set; } = null!;
        public string VendorName { get; set; } = null!;

        // Customer References
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string CustomerEmail { get; set; } = null!;
        public string CustomerFullPhoneNumber { get; set; } = null!;

        public string Number { get; set; } = null!;

        // Order References
        public Guid OrderDetailId { get; set; }

        // Customer and Vendor References
        public Guid? DeliveryAddressId { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? DeliveryRecipientName { get; set; }
        public string? DeliveryFullPhoneNumber { get; set; }

        // Refund Reason
        public RefundReason RefundReason { get; set; }
        public string? RefundReasonDetails { get; set; }
        public string? RejectionReason { get; set; } // Why request was denied

        public RefundStatus RefundStatus { get; set; }
        public decimal ReturnShippingCost { get; set; } = 0m;
        public decimal RefundAmount { get; set; } = 0m;
        public int RequestedItemsCount { get; set; }
        public int ApprovedItemsCount { get; set; }
        public string? RefundTransactionId { get; set; }
        public string? ReturnTrackingNumber { get; set; }
        public DateTime RequestDateUTC { get; set; }
        public DateTime? ApprovedDateUTC { get; set; }
        public DateTime? ReturnedDateUTC { get; set; }
        public DateTime? RefundedDateUTC { get; set; }

        public string? AdminNotes { get; set; }
        public string? AdminUserId { get; set; }
    }
}
