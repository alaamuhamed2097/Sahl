using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Order.Address
{
    /// <summary>
    /// Inline address DTO for quick address creation during checkout
    /// </summary>
    public class InlineAddressDto
    {
        [Required(ErrorMessage = "Recipient name is required")]
        [MaxLength(100)]
        public string RecipientName { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Phone code is required")]
        [MaxLength(4)]
        public string PhoneCode { get; set; } = null!;

        [Required(ErrorMessage = "City is required")]
        public Guid CityId { get; set; }
    }
}
