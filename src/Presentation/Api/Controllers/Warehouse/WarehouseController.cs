using Api.Controllers.Base;
using BL.Contracts.Service.Warehouse;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Warehouse;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.Warehouse
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : BaseController
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService, Serilog.ILogger logger)
            : base(logger)
        {
            _warehouseService = warehouseService;
        }

        /// <summary>
        /// Retrieves all warehouses.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Get()
        {
            try
            {
                var warehouses = await _warehouseService.GetAllAsync();

                return Ok(new ResponseModel<IEnumerable<WarehouseDto>>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = warehouses
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves active warehouses only.
        /// </summary>
        [HttpGet("active")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> GetActive()
        {
            try
            {
                var warehouses = await _warehouseService.GetActiveWarehousesAsync();

                return Ok(new ResponseModel<IEnumerable<WarehouseDto>>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = warehouses
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a warehouse by ID.
        /// </summary>
        /// <param name="id">The ID of the warehouse.</param>
        [HttpGet("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.InvalidInputAlert))
                    });

                var warehouse = await _warehouseService.GetByIdAsync(id);
                if (warehouse == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound))
                    });

                return Ok(new ResponseModel<WarehouseDto>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = warehouse
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches warehouses with pagination and filtering.
        /// </summary>
        /// <param name="criteriaModel">Search criteria including pagination and filters.</param>
        [HttpGet("search")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteriaModel)
        {
            try
            {
                criteriaModel.PageNumber = criteriaModel.PageNumber < 1 ? 1 : criteriaModel.PageNumber;
                criteriaModel.PageSize = criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100 ? 10 : criteriaModel.PageSize;

                var result = await _warehouseService.SearchAsync(criteriaModel);

                return Ok(new ResponseModel<PaginatedDataModel<WarehouseDto>>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Adds a new warehouse or updates an existing one.
        /// </summary>
        /// <param name="dto">The warehouse data.</param>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody] WarehouseDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid warehouse data."
                    });

                var success = await _warehouseService.SaveAsync(dto, GuidUserId);
                if (!success)
                    return Ok(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.SaveFailed))
                    });

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.SavedSuccessfully))
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a warehouse by ID (soft delete).
        /// </summary>
        /// <param name="id">The ID of the warehouse to delete.</param>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid warehouse ID."
                    });

                var success = await _warehouseService.DeleteAsync(id, GuidUserId);
                if (!success)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DeleteFailed))
                    });

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DeletedSuccessfully))
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Toggles the active status of a warehouse.
        /// </summary>
        /// <param name="id">The ID of the warehouse.</param>
        [HttpPost("toggle-status")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> ToggleStatus([FromBody] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid warehouse ID."
                    });

                var success = await _warehouseService.ToggleActiveStatusAsync(id, GuidUserId);
                if (!success)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Failed to update warehouse status."
                    });

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Warehouse status updated successfully."
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
