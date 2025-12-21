namespace Shared.DTOs.Order.Address
{
    /// <summary>
    /// DTO for address selection during checkout
    /// </summary>
    public class AddressSelectionDto
    {
        public Guid AddressId { get; set; }
        public string RecipientName { get; set; } = null!;
        public string FullPhoneNumber { get; set; } = null!;
        public string FullAddress { get; set; } = null!;
        public bool IsDefault { get; set; }
    }
}
