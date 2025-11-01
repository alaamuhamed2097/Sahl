//using Dashboard.Contracts.Notification;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.AspNetCore.SignalR.Client;
//using System.Net.Http.Json;
//using System.Security.Claims;

//namespace Dashboard.Services.Notification
//{
//    public class NotificationService : INotificationService, IAsyncDisposable
//    {
//        private readonly HttpClient _httpClient;
//        private readonly AuthenticationStateProvider _authStateProvider;
//        private HubConnection? _hubConnection;
//        private NavigationManager _navigationManager;
//        public event Action<string, string>? OnNotificationReceived;

//        public NotificationService(
//            HttpClient httpClient,
//            AuthenticationStateProvider authStateProvider,
//            NavigationManager navigationManager)
//        {
//            _httpClient = httpClient;
//            _authStateProvider = authStateProvider;
//            _navigationManager = navigationManager;
//        }

//        public async Task StartAsync()
//        {
//            if (_hubConnection == null || _hubConnection.State == HubConnectionState.Disconnected)
//            {
//                var authState = await _authStateProvider.GetAuthenticationStateAsync();

//                if (!authState.User.Identity?.IsAuthenticated ?? true)
//                {
//                    return;
//                }

//                var hubUrl = _navigationManager.BaseUri.Contains("localhost")
//                    ? "https://localhost:7049/notificationHub"
//                    : "https://mmapi.itlegend.net/notificationHub";

//                // ✅ FIXED: Simplified configuration for Blazor WebAssembly with cookie authentication
//                // The browser will automatically send cookies with the SignalR connection
//                _hubConnection = new HubConnectionBuilder()
//                    .WithUrl(hubUrl)
//                    .WithAutomaticReconnect()
//                    .Build();

//                _hubConnection.On<string, string>("ReceiveNotification", (title, message) =>
//                {
//                    OnNotificationReceived?.Invoke(title, message);
//                });

//                _hubConnection.Closed += async (error) =>
//                {
//                    // Attempt to reconnect after a delay
//                    await Task.Delay(5000);
//                    await StartAsync();
//                };

//                _hubConnection.Reconnecting += error =>
//                {
//                    return Task.CompletedTask;
//                };

//                _hubConnection.Reconnected += connectionId =>
//                {
//                    return Task.CompletedTask;
//                };

//                try
//                {
//                    await _hubConnection.StartAsync();
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"SignalR Connection Error: {ex.Message}");
//                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
//                }
//            }
//        }

//        public async Task StopAsync()
//        {
//            if (_hubConnection?.State == HubConnectionState.Connected)
//            {
//                await _hubConnection.StopAsync();
//            }
//        }

//        public async Task SendNotification(string title, string message)
//        {
//            var userId = await GetCurrentUserId();
//            if (string.IsNullOrEmpty(userId))
//            {
//                throw new InvalidOperationException("User not logged in or authentication state is invalid.");
//            }

//            var apiUrl = _navigationManager.BaseUri.Contains("localhost")
//                ? "https://localhost:7049/api/notifications/send"
//                : "https://mmapi.itlegend.net/api/notifications/send";

//            var request = new { UserId = userId, Title = title, Message = message };

//            var response = await _httpClient.PostAsJsonAsync(apiUrl, request);
//            response.EnsureSuccessStatusCode();
//        }

//        private async Task<string?> GetCurrentUserId()
//        {
//            try
//            {
//                var authState = await _authStateProvider.GetAuthenticationStateAsync();

//                if (!authState.User.Identity?.IsAuthenticated ?? true)
//                {
//                    return null;
//                }

//                // Try different claim types
//                var userId = authState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
//                          ?? authState.User.FindFirst("sub")?.Value
//                          ?? authState.User.FindFirst("userId")?.Value;

//                return userId;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error getting user ID: {ex.Message}");
//                return null;
//            }
//        }

//        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

//        public async ValueTask DisposeAsync()
//        {
//            await StopAsync();
//            if (_hubConnection != null)
//            {
//                await _hubConnection.DisposeAsync();
//            }
//        }
//    }
//}