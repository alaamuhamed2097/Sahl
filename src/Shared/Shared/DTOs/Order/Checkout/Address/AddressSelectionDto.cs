namespace Shared.DTOs.Order.Checkout.Address
{
    /// <summary>
    /// Address selection DTO for dropdowns
    /// </summary>
    public class AddressSelectionDto
    {
        public Guid AddressId { get; set; }
        public string RecipientName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public Guid CityId { get; set; }
        public string StateNameAr { get; set; } = string.Empty;
        public string StateNameEn { get; set; } = string.Empty;
        public string CityNameAr { get; set; } = string.Empty;
        public string CityNameEn { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PhoneCode { get; set; } = string.Empty;
        public string FullPhoneNumber { get; set; } = string.Empty;
        public string FullAddress { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
    }
}