using BL.Contracts.GeneralService.Notification;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Resources;
using Shared.GeneralModels;
using Shared.GeneralModels.Parameters.Notification;

namespace BL.GeneralService.Notification;

public class SignalRProviderService : ISignalRProviderService
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<SignalRProviderService> _logger;

    public SignalRProviderService(IHubContext<NotificationHub> hubContext, ILogger<SignalRProviderService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task<ResponseModel<object>> SendNotificationToUser(SignalRNotificationRequest signalRNotificationRequest)
    {
        try
        {
            _logger.LogInformation($"Attempting to send notification to user: {signalRNotificationRequest.UserId}");

            // Try both methods to ensure delivery
            // Method 1: Send to specific user (requires IUserIdProvider)
            await _hubContext.Clients.User(signalRNotificationRequest.UserId)
                .SendAsync("ReceiveNotification", signalRNotificationRequest.Title, signalRNotificationRequest.Message);

            // Method 2: Send to user group (fallback)
            //await _hubContext.Clients.Group(signalRNotificationRequest.UserId)
            //    .SendAsync("ReceiveNotification", signalRNotificationRequest.Title, signalRNotificationRequest.Message);

            _logger.LogInformation($"Notification sent successfully to user: {signalRNotificationRequest.UserId}");

            return new ResponseModel<object>()
            {
                Success = true,
                Message = NotifiAndAlertsResources.NotificationSentSuccessfully
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending notification to user: {signalRNotificationRequest.UserId}");
            throw new ApplicationException(ex.Message, ex.InnerException);
        }
    }

    // Method to send to all users
    public async Task<ResponseModel<object>> SendNotificationToAllUsers(SignalRNotificationRequest signalRNotificationRequest)
    {
        try
        {
            await _hubContext.Clients.All
                .SendAsync("ReceiveNotification", signalRNotificationRequest.Title, signalRNotificationRequest.Message);

            return new ResponseModel<object>()
            {
                Success = true,
                Message = "Notification sent to all users successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to all users");
            throw new ApplicationException(ex.Message, ex.InnerException);
        }
    }

    //public async Task<bool> SendNotificationToGroup(SignalRNotificationRequest signalRNotificationRequest)
    //{
    //    await _hubContext.Clients.User(signalRNotificationRequest.UserId).SendAsync("ReceiveNotification", signalRNotificationRequest.Title, signalRNotificationRequest.Message, signalRNotificationRequest.Type);
    //}

    //public async Task<bool> SendNotificationToAllUsers(SignalRNotificationRequest signalRNotificationRequest)
    //{
    //    await _hubContext.Clients.User(signalRNotificationRequest.UserId).SendAsync("ReceiveNotification", signalRNotificationRequest.Title, signalRNotificationRequest.Message, signalRNotificationRequest.Type);
    //}

}
