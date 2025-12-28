using Shared.DTOs.Order.Checkout.Address;

namespace BL.Contracts.Service.Order.Checkout;

/// <summary>
/// Service interface for managing customer addresses
/// </summary>
public interface ICustomerAddressService
{
    /// <summary>
    /// Get all addresses for a customer
    /// </summary>
    Task<List<CustomerAddressDto>> GetCustomerAddressesAsync(string customerId);

    /// <summary>
    /// Get a specific address by ID (with ownership validation)
    /// </summary>
    Task<CustomerAddressDto?> GetAddressByIdAsync(Guid addressId, string customerId);

    /// <summary>
    /// Get the default address for a customer
    /// </summary>
    Task<CustomerAddressDto?> GetDefaultAddressAsync(string customerId);

    /// <summary>
    /// Create a new address for a customer
    /// </summary>
    Task<CustomerAddressDto> CreateAddressAsync(string customerId, CreateCustomerAddressRequest request);

    /// <summary>
    /// Update an existing address (with ownership validation)
    /// </summary>
    Task<CustomerAddressDto?> UpdateAddressAsync(Guid addressId, string customerId, UpdateCustomerAddressRequest request);

    /// <summary>
    /// Set an address as the default address
    /// </summary>
    Task<bool> SetDefaultAddressAsync(Guid addressId, string customerId);

    /// <summary>
    /// Delete an address (soft delete with ownership validation)
    /// </summary>
    Task<bool> DeleteAddressAsync(Guid addressId, string customerId);

    /// <summary>
    /// Get formatted address details for order display
    /// </summary>
    Task<AddressSelectionDto?> GetAddressForOrderAsync(Guid addressId, string customerId);
}
