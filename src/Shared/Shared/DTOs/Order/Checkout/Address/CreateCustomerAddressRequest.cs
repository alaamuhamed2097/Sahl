namespace Shared.DTOs.Order.Checkout.Address
{
    /// <summary>
    /// Create customer address request
    /// </summary>
    public class CreateCustomerAddressRequest
    {
        public string RecipientName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PhoneCode { get; set; } = "+20"; // Default Egypt
        public string Address { get; set; } = string.Empty;
        public Guid CityId { get; set; }
        public bool IsDefault { get; set; }
    }
}