using Asp.Versioning;
using BL.Contracts.GeneralService.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.User.Admin;
using Shared.GeneralModels;

namespace Api.Controllers.v1.User
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserProfileController : Controller
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
        [ProducesResponseType(typeof(ResponseModel<AdminProfileDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserProfile()
        {
            throw new NotImplementedException();
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] AdminProfileUpdateDto ProfileDto)
        {
            throw new NotImplementedException();
        }
    }
}
