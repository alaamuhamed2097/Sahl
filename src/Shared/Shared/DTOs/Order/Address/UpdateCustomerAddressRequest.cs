using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Order.Address
{
    /// <summary>
    /// Request DTO for updating an existing address
    /// </summary>
    public class UpdateCustomerAddressRequest
    {
        [Required(ErrorMessage = "Recipient name is required")]
        [MaxLength(100, ErrorMessage = "Recipient name cannot exceed 100 characters")]
        public string RecipientName { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required")]
        [MaxLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must contain only digits")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Phone code is required")]
        [MaxLength(4, ErrorMessage = "Phone code cannot exceed 4 characters")]
        [RegularExpression(@"^\+?\d+$", ErrorMessage = "Phone code must be a valid format (e.g., +20, 20)")]
        public string PhoneCode { get; set; } = null!;

        [Required(ErrorMessage = "City is required")]
        public Guid CityId { get; set; }
    }
}
