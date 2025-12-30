using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Merchandising.CouponCode
{
    /// <summary>
    /// DTO for validating a coupon code
    /// </summary>
    public class ValidateCouponDto
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Order total is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Order total must be greater than 0")]
        public decimal OrderTotal { get; set; }

        public List<OrderItemDetailsDto>? OrderItems { get; set; }
    }

}
