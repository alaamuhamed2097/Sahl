namespace Shared.DTOs.Order.Checkout.Address
{
    /// <summary>
    /// Delivery address DTO for shipment tracking
    /// </summary>
    public class DeliveryAddressDto
    {
        public string Address { get; set; } = string.Empty;
        public string CityNameAr { get; set; } = string.Empty;
        public string CityNameEn { get; set; } = string.Empty;
        public string StateNameAr { get; set; } = string.Empty;
        public string StateNameEn { get; set; } = string.Empty;
        public string PhoneCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string RecipientName { get; set; } = string.Empty;
    }
}