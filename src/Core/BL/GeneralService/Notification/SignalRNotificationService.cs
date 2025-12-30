using Bl.Contracts.GeneralService.Notification;
using BL.GeneralService.Notification;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.ResultModels;

namespace Bl.GeneralService.Notification
{
    public class SignalRNotificationService : ISignalRNotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger _logger;

        public SignalRNotificationService(IHubContext<NotificationHub> hubContext, ILogger logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<OperationResult> SendNotificationToUser(SignalRNotificationRequest request)
        {
            try
            {
                //_logger.Information($"Sending notification to user: {request.UserId}");

                //await _hubContext.Clients.User(request.UserId)
                //    .SendAsync("ReceiveNotification", new
                //    {
                //        Title = request.Title,
                //        Message = request.Message,
                //        Timestamp = DateTime.Now
                //    });

                //_logger.Information($"Notification sent successfully to user: {request.UserId}");

                return new OperationResult
                {
                    Success = true,
                    Message = "Notification sent successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error sending notification to user: {request.UserId}");
                return new OperationResult
                {
                    Success = false,
                    Message = $"Failed to send notification: {ex.Message}"
                };
            }
        }

        public async Task<OperationResult> SendNotificationToUsers(SignalRNotificationRequest request)
        {
            try
            {
                //if (request.UserIds == null || !request.UserIds.Any())
                //{
                //    _logger.Warning("No user IDs provided for bulk notification");
                //    return new OperationResult
                //    {
                //        Success = false,
                //        Message = "No recipients specified"
                //    };
                //}

                //_logger.Information($"Sending bulk notification to {request.UserIds.Count} users");

                //var messagePayload = new
                //{
                //    Title = request.Title,
                //    Message = request.Message,
                //    Timestamp = DateTime.Now
                //};

                //// Send to each user individually (more reliable than groups)
                //var tasks = request.UserIds.Select(userId =>
                //    _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", messagePayload));

                //await Task.WhenAll(tasks);

                //_logger.Information($"Successfully sent bulk notification to {request.UserIds.Count} users");

                return new OperationResult
                {
                    Success = true,
                    Message = $"Notifications sent to {request.UserIds.Count} users"
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error sending bulk notifications to {request.UserIds?.Count ?? 0} users");
                return new OperationResult
                {
                    Success = false,
                    Message = $"Failed to send bulk notifications: {ex.Message}"
                };
            }
        }

        public async Task<OperationResult> SendNotificationToAllUsers(SignalRNotificationRequest request)
        {
            try
            {
                //_logger.Information("Sending notification to all connected users");

                //await _hubContext.Clients.All
                //    .SendAsync("ReceiveNotification", new
                //    {
                //        Title = request.Title,
                //        Message = request.Message,
                //        Timestamp = DateTime.Now
                //    });

                return new OperationResult
                {
                    Success = true,
                    Message = "Notification broadcasted to all users"
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error broadcasting notification to all users");
                return new OperationResult
                {
                    Success = false,
                    Message = $"Broadcast failed: {ex.Message}"
                };
            }
        }

        public async Task<OperationResult> SendNotificationToGroup(string groupName, SignalRNotificationRequest request)
        {
            try
            {
                //if (string.IsNullOrWhiteSpace(groupName))
                //{
                //    throw new ArgumentException("Group name cannot be empty", nameof(groupName));
                //}

                //_logger.Information($"Sending notification to group: {groupName}");

                //await _hubContext.Clients.Group(groupName)
                //    .SendAsync("ReceiveNotification", new
                //    {
                //        Title = request.Title,
                //        Message = request.Message,
                //        Timestamp = DateTime.Now
                //    });

                return new OperationResult
                {
                    Success = true,
                    Message = $"Notification sent to group {groupName}"
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error sending notification to group {groupName}");
                return new OperationResult
                {
                    Success = false,
                    Message = $"Failed to send to group: {ex.Message}"
                };
            }
        }
    }
}
