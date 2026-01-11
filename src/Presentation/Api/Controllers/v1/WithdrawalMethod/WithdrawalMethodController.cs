using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.WithdrawalMethod;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.WithdrawalMethod;
using Shared.GeneralModels;

namespace Api.Controllers.WithdrawalMethod
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WithdrawalMethodController : BaseController
    {
        private readonly IWithdrawalMethodService _WithdrawalMethodService;

        public WithdrawalMethodController(IWithdrawalMethodService WithdrawalMethodService, Serilog.ILogger logger)
        {
            _WithdrawalMethodService = WithdrawalMethodService;
        }

        /// <summary>
        /// Retrieves all WithdrawalMethods.
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAsync()
        {
            var WithdrawalMethods = await _WithdrawalMethodService.GetAllAsync();
            if (WithdrawalMethods == null)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.NoDataFound
                });
            }

            if (!WithdrawalMethods.Any())
            {
                return Ok(new ResponseModel<IEnumerable<WithdrawalMethodDto>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.NoDataFound,
                });
            }

            return Ok(new ResponseModel<IEnumerable<WithdrawalMethodDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = WithdrawalMethods
            });
        }

        /// <summary>
        /// Retrieves a WithdrawalMethod by ID.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            var WithdrawalMethod = await _WithdrawalMethodService.FindByIdAsync(id);
            if (WithdrawalMethod == null)
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.NoDataFound
                });

            return Ok(new ResponseModel<WithdrawalMethodDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = WithdrawalMethod
            });
        }

        /// <summary>
        /// Saves a WithdrawalMethod.
        /// </summary>
        [HttpPost("save")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Save([FromBody] WithdrawalMethodDto WithdrawalMethodDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            var save = await _WithdrawalMethodService.SaveAsync(WithdrawalMethodDto, GuidUserId);
            if (!save.Success)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.SaveFailed
                });

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = NotifiAndAlertsResources.SavedSuccessfully
            });
        }

        /// <summary>
        /// Deletes a WithdrawalMethod by ID.
        /// </summary>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> DeleteAsync([FromBody] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            var success = await _WithdrawalMethodService.DeleteAsync(id, GuidUserId);
            if (!success)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.DeleteFailed
                });

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DeletedSuccessfully
            });
        }
    }
}
