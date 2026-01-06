using Common.Enumerations.Order;
using Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Order.Payment.Refund
{
    public class RefundResponseDto
    {
        public Guid RefundId { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public decimal RefundAmount { get; set; }

        public RefundStatus CurrentState { get; set; } = RefundStatus.Open;

        public string? AdminComments { get; set; }
    }
}
