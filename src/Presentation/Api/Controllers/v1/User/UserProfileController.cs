using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.GeneralService.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.User;
using Shared.GeneralModels;

namespace Api.Controllers.v1.User
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserProfileController : BaseController
    {
        private readonly IUserProfileService _userProfileService;
        private readonly Serilog.ILogger _logger;

        public UserProfileController(IUserProfileService userProfileService,
            Serilog.ILogger logger)
        {
            _userProfileService = userProfileService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the profile of the logged-in user.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Authentication.
        /// </remarks>
        /// <returns>
        /// A task that represents the asynchronous operation. Returns 200 OK with user profile data,
        /// or 401 Unauthorized if the user is not authenticated.
        /// </returns>
        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseModel<UserProfileDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserProfile()
        {
            var result = await _userProfileService.GetUserProfileAsync(UserId);
            return Ok(new ResponseModel<UserProfileDto>
            {
                Success = true,
                Data = result
            });
        }

        /// <summary>
        /// Updates the profile of the logged-in user.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Authentication.
        /// </remarks>
        /// <param name="updateDto">The user profile update data transfer object.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. Returns 200 OK on successful update,
        /// or 400 Bad Request if validation fails or update fails.
        /// </returns>
        [HttpPost("profile")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseModel<UserProfileDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid input",
                    Errors = errors
                });
            }

            var result = await _userProfileService.UpdateUserProfileAsync(UserId, updateDto);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
