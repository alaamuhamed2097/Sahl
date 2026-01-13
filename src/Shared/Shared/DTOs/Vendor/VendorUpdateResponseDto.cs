namespace Shared.DTOs.Vendor
{
    public class VendorUpdateResponseDto
    {
        public Guid VendorId { get; set; }
        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string StoreName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string PhoneCode { get; set; } = null!;
        public string ProfileImagePath { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
