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
    public class ReturnMovementController : BaseController
    {
        private readonly IMortemService _mortemService;
        private readonly IMovitemsdetailService _movitemsdetailService;

        public ReturnMovementController(
            IMortemService mortemService,
            IMovitemsdetailService movitemsdetailService,
            Serilog.ILogger logger)
            : base(logger)
        {
            _mortemService = mortemService;
            _movitemsdetailService = movitemsdetailService;
        }

        /// <summary>
        /// Retrieves all return movements.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Get()
        {
            try
            {
                var returns = await _mortemService.GetAllAsync();

                return Ok(new ResponseModel<IEnumerable<MortemDto>>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = returns
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a return movement by ID.
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

                var returnMovement = await _mortemService.GetByIdAsync(id);
                if (returnMovement == null)
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound))
                    });

                // Get details
                var details = await _movitemsdetailService.GetByMortemIdAsync(id);
                returnMovement.Details = details.ToList();

                return Ok(new ResponseModel<MortemDto>
                {
                    Success = true,
                    Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
                    Data = returnMovement
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches return movements with pagination.
        /// </summary>
        [HttpGet("search")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteriaModel)
        {
            try
            {
                criteriaModel.PageNumber = criteriaModel.PageNumber < 1 ? 1 : criteriaModel.PageNumber;
                criteriaModel.PageSize = criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100 ? 10 : criteriaModel.PageSize;

                var result = await _mortemService.SearchAsync(criteriaModel);

                return Ok(new ResponseModel<PaginatedDataModel<MortemDto>>
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
                var documentNumber = await _mortemService.GenerateDocumentNumberAsync();

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
        /// Saves a return movement with details.
        /// </summary>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody] MortemDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid return movement data."
                    });

                var success = await _mortemService.SaveAsync(dto, GuidUserId);
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
        /// Updates the status of a return movement (Approve/Reject).
        /// </summary>
        [HttpPost("update-status")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusRequest request)
        {
            try
            {
                if (request.Id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid return movement ID."
                    });

                var success = await _mortemService.UpdateStatusAsync(request.Id, request.Status, GuidUserId);
                if (!success)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Failed to update status."
                    });

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Status updated successfully."
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a return movement (soft delete).
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
                        Message = "Invalid return movement ID."
                    });

                var success = await _mortemService.DeleteAsync(id, GuidUserId);
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

    public class UpdateStatusRequest
    {
        public Guid Id { get; set; }
        public int Status { get; set; } // 0: Pending, 1: Approved, 2: Rejected
    }
}
