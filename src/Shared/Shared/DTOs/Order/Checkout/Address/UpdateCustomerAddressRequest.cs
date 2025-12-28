namespace Shared.DTOs.Order.Checkout.Address
{
    /// <summary>
    /// Update customer address request
    /// </summary>
    public class UpdateCustomerAddressRequest
    {
        public Guid AddressId { get; set; }
        public string RecipientName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PhoneCode { get; set; } = "+20";
        public string Address { get; set; } = string.Empty;
        public Guid CityId { get; set; }
        public bool IsDefault { get; set; }
    }
}