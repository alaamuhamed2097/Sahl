namespace Shared.DTOs.Order.Checkout.Address
{
    /// <summary>
    /// Customer address DTO - FIXED to match entity structure
    /// </summary>
    public class CustomerAddressDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string RecipientName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PhoneCode { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public Guid CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string StateName { get; set; } = string.Empty; // Governorate
        public string CountryName { get; set; } = string.Empty; // Governorate
        public bool IsDefault { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}