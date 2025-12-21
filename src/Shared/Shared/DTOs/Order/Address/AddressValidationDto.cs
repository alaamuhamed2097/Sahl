namespace Shared.DTOs.Order.Address
{
    /// <summary>
    /// DTO for address validation response
    /// </summary>
    public class AddressValidationDto
    {
        public Guid AddressId { get; set; }
        public bool IsValid { get; set; }
        public string? Message { get; set; }
    }
}
