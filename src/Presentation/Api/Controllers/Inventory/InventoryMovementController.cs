using Api.Controllers.Base;
using BL.Contracts.Service.Inventory;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Inventory;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryMovementController : BaseController
    {
        private readonly IMoitemService _moitemService;
        private readonly IMovitemsdetailService _movitemsdetailService;

        public InventoryMovementController(
            IMoitemService moitemService,
            IMovitemsdetailService movitemsdetailService,
            Serilog.ILogger logger)
            : base(logger)
        {
            _moitemService = moitemService;
            _movitemsdetailService = movitemsdetailService;
        }

        /// <summary>
        /// Retrieves all inventory movements.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Get()
        {
            try
            {
                var movements = await _moitemService.GetAllAsync();

                return Ok(new ResponseModel<IEnumerable<MoitemDto>>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = movements
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a movement by ID.
        /// </summary>
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

                var movement = await _moitemService.GetByIdAsync(id);
                if (movement == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound))
                    });

                // Get details
                var details = await _movitemsdetailService.GetByMoitemIdAsync(id);
                movement.Details = details.ToList();

                return Ok(new ResponseModel<MoitemDto>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = movement
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a movement by document number.
        /// </summary>
        [HttpGet("by-document/{documentNumber}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> GetByDocumentNumber(string documentNumber)
        {
            try
            {
                var movement = await _moitemService.GetByDocumentNumberAsync(documentNumber);
                if (movement == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound))
                    });

                return Ok(new ResponseModel<MoitemDto>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = movement
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches movements with pagination.
        /// </summary>
        [HttpGet("search")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteriaModel)
        {
            try
            {
                criteriaModel.PageNumber = criteriaModel.PageNumber < 1 ? 1 : criteriaModel.PageNumber;
                criteriaModel.PageSize = criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100 ? 10 : criteriaModel.PageSize;

                var result = await _moitemService.SearchAsync(criteriaModel);

                return Ok(new ResponseModel<PaginatedDataModel<MoitemDto>>
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
        /// Generates a new document number.
        /// </summary>
        [HttpGet("generate-document-number")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> GenerateDocumentNumber()
        {
            try
            {
                var documentNumber = await _moitemService.GenerateDocumentNumberAsync();

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Document number generated successfully.",
                    Data = documentNumber
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Saves an inventory movement with details.
        /// </summary>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody] MoitemDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid movement data."
                    });

                var success = await _moitemService.SaveAsync(dto, GuidUserId);
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
        /// Deletes a movement (soft delete).
        /// </summary>
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
                        Message = "Invalid movement ID."
                    });

                var success = await _moitemService.DeleteAsync(id, GuidUserId);
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
    }
}
