//using Api.Controllers.Base;
//using Asp.Versioning;
//using BL.Contracts.GeneralService.Notification;
//using Common.Enumerations.User;
//using DAL.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Resources;
//using Shared.GeneralModels;
//using Shared.GeneralModels.Parameters.Notification;
//using Shared.GeneralModels.SearchCriteriaModels;
//using Shared.ResultModels;

//namespace Api.Controllers.v1.Notification
//{
//    [Route("api/v{version:apiVersion}/[controller]")]
//    [ApiController]
//    [ApiVersion("1.0")]
//    public class UserNotificationsController : BaseController
//    {
//        private readonly IUserNotificationService _userNotificationService;

        public UserNotificationsController(IUserNotificationService userNotificationService)
        {
            _userNotificationService = userNotificationService;
        }

//        /// <summary>
//        /// Retrieves all userNotifications.
//        /// </summary>
//        /// <remarks>
//        /// API Version: 1.0+
//        /// Requires Authentication.
//        /// </remarks>
//        [HttpGet]
//        [Authorize]
//        public async Task<IActionResult> Get()
//        {
//            var userNotifications = await _userNotificationService.GetAll(UserId);

//            if (userNotifications.Value == null || !userNotifications.Value.Any())
//            {
//                return Ok(new ResponseModel<UserNotificationResult<IEnumerable<UserNotificationRequest>>>
//                {
//                    Success = true,
//                    Message = NotifiAndAlertsResources.NoDataFound,
//                    Data = new UserNotificationResult<IEnumerable<UserNotificationRequest>>
//                    {
//                        Value = new List<UserNotificationRequest>(),
//                        UnReadCount = 0,
//                        TotalCount = 0
//                    }
//                });
//            }

//            return Ok(new ResponseModel<UserNotificationResult<IEnumerable<UserNotificationRequest>>>
//            {
//                Success = true,
//                Message = NotifiAndAlertsResources.DataRetrieved,
//                Data = userNotifications
//            });
//        }

//        /// <summary>
//        /// Retrieves a user Notification by ID.
//        /// </summary>
//        /// <remarks>
//        /// API Version: 1.0+
//        /// Requires Authentication.
//        /// </remarks>
//        [HttpGet("{id}")]
//        [Authorize]
//        public async Task<IActionResult> Get(Guid id)
//        {
//            if (id == Guid.Empty)
//                return BadRequest(new ResponseModel<string>
//                {
//                    Success = false,
//                    Message = NotifiAndAlertsResources.InvalidInputAlert
//                });

//            var userNotification = await _userNotificationService.FindById(id);
//            if (userNotification == null)
//                return NotFound(new ResponseModel<UserNotificationRequest>
//                {
//                    Success = false,
//                    Message = NotifiAndAlertsResources.NoDataFound
//                });

//            return Ok(new ResponseModel<UserNotificationRequest>
//            {
//                Success = true,
//                Message = NotifiAndAlertsResources.DataRetrieved,
//                Data = userNotification
//            });
//        }

//        /// <summary>
//        /// Searches user notifications with pagination and filtering.
//        /// </summary>
//        /// <remarks>
//        /// API Version: 1.0+
//        /// Requires Authentication.
//        /// </remarks>
//        /// <param name="criteria">Search criteria including pagination parameters</param>
//        [Authorize]
//        [HttpGet("search")]
//        public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
//        {
//            criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
//            criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

//            var result = await _userNotificationService.GetPage(criteria, UserId);

//            if (result.Value == null || !result.Value.Items.Any())
//            {
//                return Ok(new ResponseModel<UserNotificationResult<PaginatedDataModel<UserNotificationRequest>>>
//                {
//                    Success = true,
//                    Message = NotifiAndAlertsResources.NoDataFound,
//                    Data = result
//                });
//            }

//            return Ok(new ResponseModel<UserNotificationResult<PaginatedDataModel<UserNotificationRequest>>>
//            {
//                Success = true,
//                Message = NotifiAndAlertsResources.DataRetrieved,
//                Data = result
//            });
//        }

//        /// <summary>
//        /// Saves a user Notification.
//        /// </summary>
//        /// <remarks>
//        /// API Version: 1.0+
//        /// Requires Authentication.
//        /// </remarks>
//        [HttpPost("save")]
//        [Authorize]
//        public async Task<IActionResult> Save([FromBody] UserNotificationRequest userNotificationRequest)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(new ResponseModel<bool>
//                {
//                    Success = false,
//                    Message = NotifiAndAlertsResources.InvalidInputAlert
//                });
//            userNotificationRequest.UserId = UserId;
//            var success = await _userNotificationService.Save(userNotificationRequest, GuidUserId);
//            if (!success)
//                return BadRequest(new ResponseModel<bool>
//                {
//                    Success = false,
//                    Message = NotifiAndAlertsResources.SaveFailed
//                });

//            return Ok(new ResponseModel<bool>
//            {
//                Success = true,
//                Data = true,
//                Message = NotifiAndAlertsResources.SavedSuccessfully
//            });
//        }

//        /// <summary>
//        /// Mark user notifications as read.
//        /// </summary>
//        /// <remarks>
//        /// API Version: 1.0+
//        /// Requires Authentication.
//        /// </remarks>
//        [HttpPost("markAsRead")]
//        [Authorize]
//        public async Task<IActionResult> MarkAsRead([FromBody] IEnumerable<UserNotificationRequest> userNotificationRequests)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(new ResponseModel<UserNotificationResult<bool>>
//                {
//                    Success = false,
//                    Message = NotifiAndAlertsResources.InvalidInputAlert
//                });
//            var result = await _userNotificationService.MarkAsRead(userNotificationRequests, UserId);
//            if (!result.Value)
//                return BadRequest(new ResponseModel<UserNotificationResult<bool>>
//                {
//                    Success = false,
//                    Message = NotifiAndAlertsResources.SaveFailed
//                });

//            return Ok(new ResponseModel<UserNotificationResult<bool>>
//            {
//                Success = true,
//                Data = new UserNotificationResult<bool>()
//                {
//                    Value = true,
//                    UnReadCount = result.UnReadCount
//                },
//                Message = NotifiAndAlertsResources.SavedSuccessfully
//            });
//        }

//        /// <summary>
//        /// Deletes a user notification by ID.
//        /// </summary>
//        /// <remarks>
//        /// API Version: 1.0+
//        /// Requires Admin role.
//        /// </remarks>
//        [HttpPost("delete")]
//        [Authorize(Roles = nameof(UserRole.Admin))]
//        public async Task<IActionResult> Delete([FromBody] Guid id)
//        {
//            if (id == Guid.Empty)
//                return BadRequest(new ResponseModel<bool>
//                {
//                    Success = false,
//                    Message = NotifiAndAlertsResources.InvalidInputAlert
//                });

//            var success = await _userNotificationService.Delete(id, GuidUserId);
//            if (!success)
//                return BadRequest(new ResponseModel<bool>
//                {
//                    Success = false,
//                    Message = NotifiAndAlertsResources.DeleteFailed
//                });

//            return Ok(new ResponseModel<bool>
//            {
//                Success = true,
//                Data = true,
//                Message = NotifiAndAlertsResources.DeletedSuccessfully
//            });
//        }
//    }
//}
