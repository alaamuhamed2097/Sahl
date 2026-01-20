using Resources;
using Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.User.Customer
{
    public class CustomerRegistrationDto
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone code is required")]
        [StringLength(5, MinimumLength = 1)]
        [RegularExpression(@"^\+?[0-9]{1,5}$", ErrorMessage = "Invalid phone code format")]
        public string PhoneCode { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(15, MinimumLength = 6)]
        [RegularExpression(@"^[0-9\s\(\)\-\+]{6,15}$", ErrorMessage = "Invalid phone number format")]
        [PhoneNumber(nameof(PhoneCode), ErrorMessageResourceName = "InvalidPhoneNumber", ErrorMessageResourceType = typeof(ValidationResources))]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
