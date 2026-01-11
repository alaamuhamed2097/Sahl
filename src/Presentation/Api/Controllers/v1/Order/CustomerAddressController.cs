using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Order.Checkout;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Order.Checkout.Address;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Order
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class CustomerAddressController : BaseController
    {
        private readonly ICustomerAddressService _addressService;

        public CustomerAddressController(ICustomerAddressService addressService)
        {
            _addressService = addressService;
        }

        /// <summary>
        /// Get all addresses for the current customer
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.<br/>
        /// Returns all saved addresses for the authenticated user.
        /// </remarks>
        [HttpGet(Name = "ListAddresses")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAddresses()
        {
            var addresses = await _addressService.GetCustomerAddressesAsync(UserId);

            return Ok(new ResponseModel<List<CustomerAddressDto>>
            {
                Success = true,
                Message = "Addresses retrieved successfully.",
                Data = addresses
            });
        }

        /// <summary>
        /// Get a specific address by ID
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.<br/>
        /// Returns address details if it belongs to the authenticated user.
        /// </remarks>
        [HttpGet("{addressId:guid}", Name = "GetAddress")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetById(Guid addressId)
        {
            var address = await _addressService.GetAddressByIdAsync(addressId, UserId);

            if (address == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Address not found or you don't have permission to access it."
                });
            }

            return Ok(new ResponseModel<CustomerAddressDto>
            {
                Success = true,
                Message = "Address retrieved successfully.",
                Data = address
            });
        }

        /// <summary>
        /// Get the default address for the current customer
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.<br/>
        /// Returns the default delivery address if set.
        /// </remarks>
        [HttpGet("~/api/v{version:apiVersion}/[controller]/default", Name = "GetDefaultAddress")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDefault()
        {
            var address = await _addressService.GetDefaultAddressAsync(UserId);

            if (address == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "No default address found."
                });
            }

            return Ok(new ResponseModel<CustomerAddressDto>
            {
                Success = true,
                Message = "Default address retrieved successfully.",
                Data = address
            });
        }

        /// <summary>
        /// Create a new address
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.<br/>
        /// Creates a new delivery address for the authenticated user.<br/>
        /// If this is the first address, it will automatically be set as default.
        /// </remarks>
        [HttpPost(Name = "CreateAddress")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCustomerAddressRequest request)
        {
            var address = await _addressService.CreateAddressAsync(UserId, request);

            return CreatedAtRoute(
                "GetAddress",
                new { addressId = address.Id },
                new ResponseModel<CustomerAddressDto>
                {
                    Success = true,
                    Message = "Address created successfully.",
                    Data = address
                });
        }

        /// <summary>
        /// Update an existing address
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.<br/>
        /// Updates address details. User can only update their own addresses.
        /// </remarks>
        [HttpPut("{addressId:guid}", Name = "UpdateAddress")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update(Guid addressId, [FromBody] UpdateCustomerAddressRequest request)
        {
            var address = await _addressService.UpdateAddressAsync(addressId, UserId, request);

            if (address == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Address not found or you don't have permission to update it."
                });
            }

            return Ok(new ResponseModel<CustomerAddressDto>
            {
                Success = true,
                Message = "Address updated successfully.",
                Data = address
            });
        }

        /// <summary>
        /// Set an address as default
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.<br/>
        /// Sets the specified address as the default delivery address.<br/>
        /// Any previously default address will be unmarked.
        /// </remarks>
        [HttpPut("{addressId:guid}/default", Name = "SetDefaultAddress")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> MarkAsDefault(Guid addressId)
        {
            var result = await _addressService.SetDefaultAddressAsync(addressId, UserId);

            if (!result)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Address not found or you don't have permission to modify it."
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = true,
                Message = "Default address updated successfully.",
                Data = new { AddressId = addressId }
            });
        }

        /// <summary>
        /// Delete an address
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+<br/>
        /// Requires Customer role.<br/>
        /// Soft deletes the address. User can only delete their own addresses.<br/>
        /// If deleting the default address and other addresses exist, another will be set as default.
        /// </remarks>
        [HttpDelete("{addressId:guid}", Name = "DeleteAddress")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid addressId)
        {
            var result = await _addressService.DeleteAddressAsync(addressId, UserId);

            if (!result)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Address not found or you don't have permission to delete it."
                });
            }

            return Ok(new ResponseModel<object>
            {
                Success = true,
                Message = "Address deleted successfully.",
                Data = new { AddressId = addressId }
            });
        }
    }
}