using Shared.DTOs.Order.OrderProcessing;
using System.ComponentModel.DataAnnotations;

namespace Shared.GeneralModels.Parameters
{
    public class ApplyCouponCodeRequest
    {
        [Required]
        public string Code { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public IEnumerable<OrderItemDto> OrderItems { get; set; } = null!;
    }
}
