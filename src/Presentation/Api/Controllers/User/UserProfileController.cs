using BL.Contracts.GeneralService.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.User.Admin;
using Shared.GeneralModels;

namespace Api.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
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
            //try
            //{
            //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //    var userProfile = await _userProfileService.GetAdminProfileAsync(userId);
            //    return Ok(new ResponseModel<AdminProfileDto> 
            //    {
            //        Success=true,
            //        Data = userProfile,
            //    });
            //}
            //catch (Exception ex)
            //{
            //    var errors =new List<string>();
            //    foreach (var error in ex.InnerException.Data)
            //    {
            //        errors.Add(error.ToString());
            //    }
            //    _logger.Error(ex, "An error occurred while retrieving user profile.");
            //    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<AdminProfileDto>
            //    {
            //        Success = false,
            //        Message = "An unexpected error occurred. Please try again later.",
            //        StatusCode = StatusCodes.Status500InternalServerError,
            //        Errors = errors
            //    });
            //}

            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the profile of the logged-in user.
        /// </summary>
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
            //try
            //{
            //    if (!ModelState.IsValid)
            //    {
            //        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //        return Ok(new ResponseModel<string> { Success = false, Errors = errors });
            //    }

            //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //    var result = await _userProfileService.UpdateAdminProfileAsync(userId, ProfileDto, new Guid(userId));

            //    if (result.Success)
            //    {
            //        return Ok(new ResponseModel<string> { Success = true, Message = "Profile updated successfully." });
            //    }

            //    return Ok(new ResponseModel<string> { Success = false, Message = result.Message });
            //}
            //catch (Exception ex)
            //{
            //    _logger.Error(ex, "An error occurred while updating user profile.");
            //    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<object>
            //    {
            //        Success = false,
            //        Message = "An unexpected error occurred. Please try again later."
            //    });
            //}

            throw new NotImplementedException();
        }
    }
}
