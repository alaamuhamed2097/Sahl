using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BL.GeneralService.Notification;

public class NotificationHub : Hub
{
    private readonly ILogger<NotificationHub> _logger;

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }
    public override async Task OnConnectedAsync()
    {
        // Log the connection Adding
        var userId = Context.UserIdentifier;
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Log the connection Removing
        var userId = Context.UserIdentifier;
        await base.OnDisconnectedAsync(exception);
    }
}
