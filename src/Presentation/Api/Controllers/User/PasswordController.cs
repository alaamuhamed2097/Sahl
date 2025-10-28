using BL.Contracts.GeneralService.CMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.User;
using Shared.GeneralModels;
using System.Security.Claims;

namespace Api.Controllers
{
    /// <summary>
    /// Provides endpoints for user password management, including password reset and change functionalities.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly IUserAuthenticationService _authenticationService;
        private readonly Serilog.ILogger _logger;

        public PasswordController(IUserAuthenticationService authenticationService, Serilog.ILogger logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        /// <summary>
        /// Changes the password of the logged-in user.
        /// </summary>
        /// <param name="changeDto">The change password data transfer object containing old and new passwords.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. Returns 200 OK on successful password change,
        /// or 400 Bad Request if validation fails or the old password is incorrect.
        /// </returns>
        [HttpPut("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Explicitly convert the IEnumerable<string> to a IEnumerable<string>
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Ok(new ResponseModel<string> { Success = false, Errors = errors });
                }

                // Extract user ID from the token claims
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Change the password
                var result = await _authenticationService.ChangePasswordAsync(userId, changeDto.CurrentPassword, changeDto.NewPassword);

                if (result.Success)
                {
                    return Ok(new { Success = true, Message = "Password changed successfully." });
                }

                return Ok(new { Success = false, result.Message });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while changing the password.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Message = "An unexpected error occurred. Please try again later." });
            }
        }
    }
}
