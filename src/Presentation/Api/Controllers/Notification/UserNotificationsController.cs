using Api.Controllers.Base;
using BL.Contracts.GeneralService.Notification;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.GeneralModels;
using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.SearchCriteriaModels;
using Shared.ResultModels;

namespace Api.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserNotificationsController : BaseController
    {
        private readonly IUserNotificationService _userNotificationService;

        public UserNotificationsController(Serilog.ILogger logger, IUserNotificationService userNotificationService)
            : base(logger)
        {
            _userNotificationService = userNotificationService;
        }

        /// <summary>
        /// Retrieves all userNotifications.
        /// </summary>
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                var userNotifications = _userNotificationService.GetAll(UserId);
                
                // Return OK even when there are no notifications instead of 404
                if (userNotifications.Value == null || !userNotifications.Value.Any())
                {
                    return Ok(new ResponseModel<UserNotificationResult<IEnumerable<UserNotificationRequest>>>
                    {
                        Success = true,
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = new UserNotificationResult<IEnumerable<UserNotificationRequest>>
                                {
                            Value = new List<UserNotificationRequest>(),
                                 UnReadCount = 0,
                                TotalCount = 0
                               }
                       });
                }

                return Ok(new ResponseModel<UserNotificationResult<IEnumerable<UserNotificationRequest>>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = userNotifications
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Retrieves a user Notification by ID.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var userNotification = _userNotificationService.FindById(id);
                if (userNotification == null)
                    return NotFound(new ResponseModel<UserNotificationRequest>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.NoDataFound
                    });

                return Ok(new ResponseModel<UserNotificationRequest>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = userNotification
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches user notifications with pagination and filtering
        /// </summary>
        /// <param name="criteria">Search criteria including pagination parameters</param>
        [Authorize]
        [HttpGet("search")]
        public IActionResult Search([FromQuery] BaseSearchCriteriaModel criteria)
        {
            try
            {
                // Validate and set default pagination values if not provided
                criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
                criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

                var result = _userNotificationService.GetPage(criteria, UserId);

                if (result.Value == null || !result.Value.Items.Any())
                {
                    return Ok(new ResponseModel<UserNotificationResult<PaginatedDataModel<UserNotificationRequest>>>
                    {
                        Success = true,
                        Message = NotifiAndAlertsResources.NoDataFound,
                        Data = result
                    });
                }

                return Ok(new ResponseModel<UserNotificationResult<PaginatedDataModel<UserNotificationRequest>>>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.DataRetrieved,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Saves a user Notification.
        /// </summary>
        [HttpPost("save")]
        [Authorize]
        public async Task<IActionResult> Save([FromBody] UserNotificationRequest userNotificationRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<bool>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });
                userNotificationRequest.UserId = UserId;
                var success = await _userNotificationService.Save(userNotificationRequest, GuidUserId);
                if (!success)
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Mark user notifications as read .
        /// </summary>
        [HttpPost("markAsRead")]
        [Authorize]
        public async Task<IActionResult> MarkAsRead([FromBody] IEnumerable<UserNotificationRequest> userNotificationRequests)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseModel<UserNotificationResult<bool>>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });
                var result = await _userNotificationService.MarkAsRead(userNotificationRequests, UserId);
                if (!result.Value)
                    return BadRequest(new ResponseModel<UserNotificationResult<bool>>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.SaveFailed
                    });

                return Ok(new ResponseModel<UserNotificationResult<bool>>
                {
                    Success = true,
                    Data = new UserNotificationResult<bool>()
                    {
                        Value = true,
                        UnReadCount = result.UnReadCount
                    },
                    Message = NotifiAndAlertsResources.SavedSuccessfully
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a user notification by ID.
        /// </summary>
        [HttpPost("delete")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest(new ResponseModel<bool>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.InvalidInputAlert
                    });

                var success = await _userNotificationService.Delete(id, GuidUserId);
                if (!success)
                    return BadRequest(new ResponseModel<bool>
                    {
                        Success = false,
                        Message = NotifiAndAlertsResources.DeleteFailed
                    });

                return Ok(new ResponseModel<bool>
                {
                    Success = true,
                    Data = true,
                    Message = NotifiAndAlertsResources.DeletedSuccessfully
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
