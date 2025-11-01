using BL.Contracts.GeneralService.Notification;
using BL.GeneralService.Notification;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Resources;
using Shared.GeneralModels;
using Shared.GeneralModels.Parameters.Notification;

namespace Api.Controllers.Notification
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationController(INotificationService notificationService, IHubContext<NotificationHub> hubContext)
        {
            _notificationService = notificationService;
            _hubContext = hubContext;
        }

        [HttpPost("send")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            var result = await _notificationService.SendNotificationAsync(request);
            if (!result.Success)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = result.Message
                });
            }
            return Ok(new ResponseModel<object>
            {
                Success = true,
                Message = NotifiAndAlertsResources.NotificationSentSuccessfully,
            });
        }
        [HttpPost("sendToAllUser")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> SendNotificationToAllUser(SignalRNotificationRequest signalRNotificationRequest)
        {
            try
            {
                // Method 1: Send to All users
                await _hubContext.Clients.All
                    .SendAsync("ReceiveNotification", signalRNotificationRequest.Title, signalRNotificationRequest.Message);

                // Method 2: Send to user group (fallback)
                //await _hubContext.Clients.Group(signalRNotificationRequest.UserId)
                //    .SendAsync("ReceiveNotification", signalRNotificationRequest.Title, signalRNotificationRequest.Message);


                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = NotifiAndAlertsResources.NotificationSentSuccessfully,
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
        }

    }
}
