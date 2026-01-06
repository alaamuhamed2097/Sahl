using AutoMapper;
using BL.Contracts.Service.Order.Checkout;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Order;
using Serilog;
using Shared.DTOs.Order.Checkout.Address;

namespace BL.Services.Order.Checkout;

/// <summary>
/// Service for managing customer delivery addresses
/// Handles CRUD operations, default address management, and ownership validation
/// Uses AutoMapper for entity-DTO conversions
/// </summary>
public class CustomerAddressService : ICustomerAddressService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CustomerAddressService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<CustomerAddressDto>> GetCustomerAddressesAsync(string customerId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));

            var repo = _unitOfWork.TableRepository<TbCustomerAddress>();
            var addresses = await repo.GetAsync(
                a => a.UserId == customerId && !a.IsDeleted,
                orderBy: q => q.OrderByDescending(a => a.IsDefault)
                               .ThenByDescending(a => a.CreatedDateUtc)
            );

            // Use AutoMapper to convert list
            return _mapper.Map<List<CustomerAddressDto>>(addresses);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error retrieving addresses for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<CustomerAddressDto?> GetAddressByIdAsync(Guid addressId, string customerId)
    {
        try
        {
            if (addressId == Guid.Empty)
                throw new ArgumentException("Address ID cannot be empty.", nameof(addressId));
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));

            var repo = _unitOfWork.TableRepository<TbCustomerAddress>();
            var address = await repo.FindAsync(
                a => a.Id == addressId && a.UserId == customerId && !a.IsDeleted
            );

            // Use AutoMapper for single object
            return address == null ? null : _mapper.Map<CustomerAddressDto>(address);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error retrieving address {AddressId} for customer {CustomerId}", addressId, customerId);
            throw;
        }
    }

    public async Task<CustomerAddressDto?> GetDefaultAddressAsync(string customerId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));

            var repo = _unitOfWork.TableRepository<TbCustomerAddress>();
            var address = await repo.FindAsync(
                a => a.UserId == customerId && a.IsDefault && !a.IsDeleted
            );

            return address == null ? null : _mapper.Map<CustomerAddressDto>(address);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error retrieving default address for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<CustomerAddressDto> CreateAddressAsync(string customerId, CreateCustomerAddressRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var customerIdGuid = Guid.Parse(customerId);
            var repo = _unitOfWork.TableRepository<TbCustomerAddress>();

            // Check if this is the first address
            var existingAddresses = await repo.GetAsync(
                a => a.UserId == customerId && !a.IsDeleted
            );

            bool isFirstAddress = !existingAddresses.Any();
            bool shouldBeDefault = isFirstAddress || request.IsDefault;

            // If setting as default, unmark existing default
            if (shouldBeDefault)
            {
                await UnmarkExistingDefaultAsync(customerId, customerIdGuid);
            }

            // Use AutoMapper to create entity from request DTO
            var address = _mapper.Map<TbCustomerAddress>(request);

            // Set properties that AutoMapper ignores
            address.Id = Guid.NewGuid();
            address.UserId = customerId;
            address.IsDefault = shouldBeDefault;
            address.CreatedDateUtc = DateTime.UtcNow;
            address.CreatedBy = customerIdGuid;

            await repo.CreateAsync(address, customerIdGuid);

            _logger.Information(
                "Address {AddressId} created for customer {CustomerId}. IsDefault: {IsDefault}",
                address.Id, customerId, shouldBeDefault
            );

            // Map back to DTO
            return _mapper.Map<CustomerAddressDto>(address);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating address for customer {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<CustomerAddressDto?> UpdateAddressAsync(
        Guid addressId,
        string customerId,
        UpdateCustomerAddressRequest request)
    {
        try
        {
            if (addressId == Guid.Empty)
                throw new ArgumentException("Address ID cannot be empty.", nameof(addressId));
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var customerIdGuid = Guid.Parse(customerId);
            var repo = _unitOfWork.TableRepository<TbCustomerAddress>();

            // Get address with ownership validation
            var address = await repo.FindAsync(
                a => a.Id == addressId && a.UserId == customerId && !a.IsDeleted
            );

            if (address == null)
            {
                _logger.Warning(
                    "Address {AddressId} not found or does not belong to customer {CustomerId}",
                    addressId, customerId
                );
                return null;
            }

            // Use AutoMapper to update entity from DTO
            _mapper.Map(request, address);

            // Set update timestamp
            address.UpdatedDateUtc = DateTime.UtcNow;

            var result = await repo.UpdateAsync(address, customerIdGuid);

            if (!result.Success)
            {
                throw new InvalidOperationException("Failed to update address.");
            }

            _logger.Information("Address {AddressId} updated for customer {CustomerId}", addressId, customerId);

            return _mapper.Map<CustomerAddressDto>(address);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error updating address {AddressId} for customer {CustomerId}", addressId, customerId);
            throw;
        }
    }

    public async Task<bool> SetDefaultAddressAsync(Guid addressId, string customerId)
    {
        try
        {
            if (addressId == Guid.Empty)
                throw new ArgumentException("Address ID cannot be empty.", nameof(addressId));
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));

            var customerIdGuid = Guid.Parse(customerId);
            var repo = _unitOfWork.TableRepository<TbCustomerAddress>();

            // Validate ownership
            var address = await repo.FindAsync(
                a => a.Id == addressId && a.UserId == customerId && !a.IsDeleted
            );

            if (address == null)
            {
                _logger.Warning(
                    "Address {AddressId} not found or does not belong to customer {CustomerId}",
                    addressId, customerId
                );
                return false;
            }

            // Unmark existing default
            await UnmarkExistingDefaultAsync(customerId, customerIdGuid);

            // Set new default
            address.IsDefault = true;
            address.UpdatedDateUtc = DateTime.UtcNow;

            var result = await repo.UpdateAsync(address, customerIdGuid);

            if (result.Success)
            {
                _logger.Information(
                    "Address {AddressId} set as default for customer {CustomerId}",
                    addressId, customerId
                );
            }

            return result.Success;
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error setting address {AddressId} as default for customer {CustomerId}",
                addressId, customerId
            );
            throw;
        }
    }

    public async Task<bool> DeleteAddressAsync(Guid addressId, string customerId)
    {
        try
        {
            if (addressId == Guid.Empty)
                throw new ArgumentException("Address ID cannot be empty.", nameof(addressId));
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));

            var customerIdGuid = Guid.Parse(customerId);
            var repo = _unitOfWork.TableRepository<TbCustomerAddress>();

            // Validate ownership
            var address = await repo.FindAsync(
                a => a.Id == addressId && a.UserId == customerId && !a.IsDeleted
            );

            if (address == null)
            {
                _logger.Warning(
                    "Address {AddressId} not found or does not belong to customer {CustomerId}",
                    addressId, customerId
                );
                return false;
            }

            bool wasDefault = address.IsDefault;

            // Soft delete
            address.IsDeleted = true;
            address.IsDefault = false;
            address.UpdatedDateUtc = DateTime.UtcNow;

            var result = await repo.UpdateAsync(address, customerIdGuid);

            if (result.Success)
            {
                _logger.Information("Address {AddressId} deleted for customer {CustomerId}", addressId, customerId);

                // If we deleted the default address, set another as default
                if (wasDefault)
                {
                    await SetFirstAvailableAsDefaultAsync(customerId, customerIdGuid);
                }
            }

            return result.Success;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error deleting address {AddressId} for customer {CustomerId}", addressId, customerId);
            throw;
        }
    }

    public async Task<bool> ValidateAddressOwnershipAsync(Guid addressId, string customerId)
    {
        try
        {
            if (addressId == Guid.Empty || string.IsNullOrWhiteSpace(customerId))
                return false;

            var repo = _unitOfWork.TableRepository<TbCustomerAddress>();
            var address = await repo.FindAsync(
                a => a.Id == addressId && a.UserId == customerId && !a.IsDeleted
            );

            return address != null;
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error validating address ownership {AddressId} for customer {CustomerId}",
                addressId, customerId
            );
            return false;
        }
    }

    public async Task<AddressSelectionDto?> GetAddressForOrderAsync(Guid addressId, string customerId)
    {
        try
        {
            if (addressId == Guid.Empty)
                throw new ArgumentException("Address ID cannot be empty.", nameof(addressId));
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));

            var repo = _unitOfWork.TableRepository<TbCustomerAddress>();
            var address = await repo.FindAsync(
                a => a.Id == addressId && a.UserId == customerId && !a.IsDeleted
            );

            if (address == null)
                return null;

            // Use AutoMapper to convert to AddressSelectionDto
            return _mapper.Map<AddressSelectionDto>(address);
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error getting address {AddressId} for order for customer {CustomerId}",
                addressId, customerId
            );
            throw;
        }
    }

    // ==================== PRIVATE HELPERS ====================

    /// <summary>
    /// Unmarks any existing default address for a customer
    /// </summary>
    private async Task UnmarkExistingDefaultAsync(string customerId, Guid updaterId)
    {
        var repo = _unitOfWork.TableRepository<TbCustomerAddress>();
        var currentDefault = await repo.FindAsync(
            a => a.UserId == customerId && a.IsDefault && !a.IsDeleted
        );

        if (currentDefault != null)
        {
            currentDefault.IsDefault = false;
            currentDefault.UpdatedDateUtc = DateTime.UtcNow;
            await repo.UpdateAsync(currentDefault, updaterId);
        }
    }

    /// <summary>
    /// Sets the first available address as default (used after deleting default address)
    /// </summary>
    private async Task SetFirstAvailableAsDefaultAsync(string customerId, Guid updaterId)
    {
        var repo = _unitOfWork.TableRepository<TbCustomerAddress>();
        var addresses = await repo.GetAsync(
            a => a.UserId == customerId && !a.IsDeleted,
            orderBy: q => q.OrderByDescending(a => a.CreatedDateUtc)
        );

        var firstAddress = addresses.FirstOrDefault();
        if (firstAddress != null)
        {
            firstAddress.IsDefault = true;
            firstAddress.UpdatedDateUtc = DateTime.UtcNow;
            await repo.UpdateAsync(firstAddress, updaterId);

            _logger.Information(
                "Address {AddressId} automatically set as default for customer {CustomerId} after deletion",
                firstAddress.Id, customerId
            );
        }
    }
}