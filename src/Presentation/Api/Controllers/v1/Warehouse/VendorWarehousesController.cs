using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.VendorWarehouse;
using BL.Contracts.Service.Warehouse;
using Common.Enumerations.User;
using Common.Filters;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Vendor;
using Shared.DTOs.Warehouse;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.Warehouse
{
    [Route("api/v{version:apiVersion}/vendors")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VendorWarehousesController : BaseController
    {
        private readonly IVendorWarehouseService _warehouseService;

        public VendorWarehousesController(IVendorWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        /// <summary>
        /// Get all available (active, not deleted) warehouses that the vendor can use them.
        /// </summary>
        /// <returns>List of warehouses</returns>
        [HttpGet("me/warehouses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles =  nameof(UserRole.Vendor))]
        public async Task<IActionResult> GetAvailableWarehousesAsync()
        {
            var warehouses = await _warehouseService
                .GetVendorAvailableWarehousesByUserIdAsync(UserId);

            return Ok(warehouses);
        }

        /// <summary>
        /// Get market warehouse.
        /// </summary>
        /// <returns>List of warehouses</returns>
        [HttpGet("market/warehouse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles =  nameof(UserRole.Admin))]
        public async Task<IActionResult> GetMarketWarehousesAsync()
        {
            var warehouse = await _warehouseService
                .GetMarketWarehousesAsync();

            return Ok(warehouse);
        }
    }
}
