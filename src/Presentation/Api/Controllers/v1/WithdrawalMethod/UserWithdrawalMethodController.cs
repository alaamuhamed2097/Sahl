using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.WithdrawalMethod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.WithdrawalMethod;
using Shared.GeneralModels;

namespace Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserWithdrawalMethodController : BaseController
    {
        private readonly IUserWithdrawalMethodService _userWithdrawalService;

        public UserWithdrawalMethodController(IUserWithdrawalMethodService userWithdrawalService, Serilog.ILogger logger)
        {
            _userWithdrawalService = userWithdrawalService;
        }

        /// <summary>
        /// Retrieves all WithdrawalMethods.
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var WithdrawalMethods = await _userWithdrawalService.GetAllWithdrawalFieldsValues(UserId);
            if (WithdrawalMethods == null || !WithdrawalMethods.Any())
                return NotFound(new ResponseModel<WithdrawalMethodsFieldsValuesDto>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.NoDataFound
                });

            return Ok(new ResponseModel<IEnumerable<WithdrawalMethodsFieldsValuesDto>>
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
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            var WithdrawalMethod = await _userWithdrawalService.FindWithdrawalFieldsValuesById(id, UserId);
            if (WithdrawalMethod == null)
                return NotFound(new ResponseModel<WithdrawalMethodsFieldsValuesDto>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.NoDataFound
                });

            return Ok(new ResponseModel<WithdrawalMethodsFieldsValuesDto>
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
        [Authorize]
        public async Task<IActionResult> Save([FromBody] UserWithdrawalMethodDto WithdrawalMethodDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<bool>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });
            WithdrawalMethodDto.UserId = GuidUserId.ToString();
            var success = await _userWithdrawalService.SaveAsync(WithdrawalMethodDto, GuidUserId);
            if (!success.Success)
                return BadRequest(new ResponseModel<bool>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.SaveFailed
                });

            return Ok(new ResponseModel<bool>
            {
                Success = true,
                Data = true,
                Message = NotifiAndAlertsResources.SavedSuccessfully
            });
        }

        /// <summary>
        /// Deletes a user WithdrawalMethod by ID.
        /// </summary>
        [HttpPost("delete")]
        [Authorize]
        public async Task<IActionResult> Delete([FromBody] Guid WithdrawalMethodId)
        {
            if (WithdrawalMethodId == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.InvalidInputAlert
                });

            var success = await _userWithdrawalService.DeleteAsync(WithdrawalMethodId, GuidUserId);
            if (!success)
                return BadRequest(new ResponseModel<bool>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.DeleteFailed
                });

            return Ok(new ResponseModel<bool>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DeletedSuccessfully
            });
        }
    }
}