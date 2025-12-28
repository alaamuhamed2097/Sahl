namespace Shared.DTOs.Order.Checkout.Address
{
    /// <summary>
    /// Inline address DTO for quick checkout
    /// </summary>
    public class InlineAddressDto
    {
        public string RecipientName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PhoneCode { get; set; } = "+20";
        public string Address { get; set; } = string.Empty;
        public Guid CityId { get; set; }
    }
}