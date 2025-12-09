using Api.Controllers.Base;
using Asp.Versioning;
using BL.Contracts.GeneralService.UserManagement;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.User.Admin;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.User
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AdminController : BaseController
    {
        private readonly IUserProfileService _adminService;
        private readonly IUserRegistrationService _registeredServices;

        public AdminController(IUserProfileService adminService, IUserRegistrationService registeredServices)
        {
            _adminService = adminService;
            _registeredServices = registeredServices;
        }

        /// <summary>
        /// Get all admins.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _adminService.GetAllAdminsAsync();
            if (users == null || !users.Any())
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "No Admins found."
                });

            return Ok(new ResponseModel<IEnumerable<AdminProfileDto>>
            {
                Success = true,
                Message = "Admins retrieved successfully.",
                Data = users
            });
        }

        /// <summary>
        /// Get admin by ID.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid Admin ID."
                });

            var user = await _adminService.GetAdminProfileAsync(id.ToString());
            if (user == null)
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Admin not found."
                });

            return Ok(new ResponseModel<AdminProfileDto>
            {
                Success = true,
                Message = "Admin retrieved successfully.",
                Data = user
            });
        }

        /// <summary>
        /// Search admins with pagination.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
            criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

            var result = await _adminService.GetAdminsPage(criteria);

            if (result == null || !result.Items.Any())
            {
                return NotFound(new ResponseModel<PaginatedDataModel<string>>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.NoDataFound
                });
            }

            return Ok(new ResponseModel<PaginatedDataModel<AdminProfileDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = result
            });
        }

        /// <summary>
        /// Create new admin.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdminRegistrationDto Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Invalid Admin data."

                });

            var user = await _registeredServices.RegisterAdminAsync(Dto, GuidUserId);
            if (user == null)
                return BadRequest(new ResponseModel<bool>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.SaveFailed
                });

            return Ok(new ResponseModel<bool>
            {
                Success = true,
                Message = NotifiAndAlertsResources.SavedSuccessfully,
                Data = user.Success
            });
        }

        /// <summary>
        /// Update admin profile.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AdminProfileUpdateDto Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Invalid Admin data."

                });

            var user = await _adminService.UpdateAdminProfileAsync(id.ToString(), Dto, GuidUserId);
            if (user == null)
                return BadRequest(new ResponseModel<bool>
                {
                    Success = false,
                    Message = NotifiAndAlertsResources.SaveFailed,
                    Errors = user.Errors
                });

            return Ok(new ResponseModel<AdminProfileDto>
            {
                Success = true,
                Message = NotifiAndAlertsResources.SavedSuccessfully,
                Data = user.Data
            });
        }

        /// <summary>
        /// Delete admin account.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Invalid Admin ID."
                });

            var success = await _adminService.DeleteAccount(id, GuidUserId);
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
