namespace Shared.DTOs.User.Customer;

/// <summary>
/// DTO for Customer information in Admin views
/// Contains essential customer details for order management
/// </summary>
public class AdminCustomerInfoDto
{
    /// <summary>
    /// Customer unique identifier
    /// </summary>
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    /// Customer full name
    /// </summary>
    public string CustomerFullName { get; set; } = string.Empty;

    /// <summary>
    /// Customer email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Customer phone number
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Customer phone code
    /// </summary>
    public string PhoneCode { get; set; } = string.Empty;
}