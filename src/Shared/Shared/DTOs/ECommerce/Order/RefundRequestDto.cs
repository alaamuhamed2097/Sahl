using Common.Enumerations.Payment;
using Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.ECommerce.Order
{
    public class RefundRequestDto
    {
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(500, MinimumLength = 2, ErrorMessageResourceName = "OutOfMaxLength", ErrorMessageResourceType = typeof(ValidationResources))]
        public string RefundReason { get; set; } = string.Empty;

        public PaymentGatewayMethod? RefundMethod { get; set; }

        public Guid OrderId { get; set; } = Guid.Empty;
    }
}
