namespace Shared.DTOs.Order.Address
{
    /// <summary>
    /// DTO for customer address information
    /// </summary>
    public class CustomerAddressDto
    {
        public Guid Id { get; set; }
        public string RecipientName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string PhoneCode { get; set; } = null!;
        public Guid CityId { get; set; }
        public string? CityName { get; set; }
        public string? StateName { get; set; }
        public string? CountryName { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
