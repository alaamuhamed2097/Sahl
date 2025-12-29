namespace Shared.DTOs.Order.Checkout.Address
{
    /// <summary>
    /// Delivery address DTO for shipment tracking
    /// </summary>
    public class DeliveryAddressDto
    {
        public string Address { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string StateName { get; set; } = string.Empty;
        public string PhoneCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string RecipientName { get; set; } = string.Empty;
    }
}