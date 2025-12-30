using Bl.Contracts.GeneralService.Notification;
using BL.Contracts.GeneralService;
using BL.Contracts.GeneralService.CMS;
using Common.Enumerations.Notification;
using Serilog;
using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.ResultModels;

namespace BL.GeneralService;

public class VerificationCodeService : IVerificationCodeService
{
    private readonly ICacheService _memoryCache;
    private readonly INotificationService _notificationService;
    private readonly ILogger _logger;

    private const int CodeExpirationTimeInMinutes = 15;
    private const int AttemptWindowInMinutes = 60;

    public VerificationCodeService(
        ICacheService memoryCache,
        INotificationService notificationService,
        ILogger logger)
    {
        _memoryCache = memoryCache;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<OperationResult> SendCodeAsync(string recipient, NotificationChannel channel, NotificationType type = NotificationType.EmailVerification, Dictionary<string, string> parameters = null)
    {
        try
        {
            var attemptKey = $"{recipient}_attempts";
            var lastSentKey = $"{recipient}_last_sent";

            var now = DateTime.UtcNow;

            // Load attempt data
            var attempts = _memoryCache.Get<int?>(attemptKey) ?? 0;
            var lastSentTime = _memoryCache.Get<DateTime?>(lastSentKey);

            // Reset attempts if more than 1 hour has passed
            if (lastSentTime.HasValue && (now - lastSentTime.Value).TotalMinutes > AttemptWindowInMinutes)
            {
                attempts = 0; // Reset attempt count
                _memoryCache.Remove(attemptKey); // Remove old attempts cache
            }

            // Calculate cooldown based on attempts
            int cooldownSeconds = attempts switch
            {
                0 => 0,
                1 => 60,
                2 => 120,
                3 => 300,
                _ => 600 // Max cap at 10 min
            };

            // Check cooldown
            if (lastSentTime.HasValue && (now - lastSentTime.Value).TotalSeconds < cooldownSeconds)
            {
                var waitTime = cooldownSeconds - (int)(now - lastSentTime.Value).TotalSeconds;
                return new OperationResult
                {
                    Success = false,
                    Message = $"Please wait {waitTime} seconds before requesting another code"
                };
            }

            // Generate and cache code
            var code = GenerateAndCacheCode(recipient);

            // Prepare parameters
            var templateParameters = parameters ?? new Dictionary<string, string>();
            templateParameters["code"] = code;
            templateParameters["expiry_minutes"] = CodeExpirationTimeInMinutes.ToString();

            var notificationRequest = new NotificationRequest
            {
                Recipient = recipient,
                Channel = channel,
                Type = type,
                Parameters = templateParameters
            };

            var sendResult = await _notificationService.SendNotificationAsync(notificationRequest);

            if (sendResult.Success)
            {
                // Update attempts and last sent time
                _memoryCache.Set(attemptKey, attempts + 1, TimeSpan.FromMinutes(AttemptWindowInMinutes));
                _memoryCache.Set(lastSentKey, now, TimeSpan.FromMinutes(AttemptWindowInMinutes));
            }

            return sendResult;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error sending verification code to {recipient}");
            return new OperationResult
            {
                Success = false,
                Message = "Failed to send verification code"
            };
        }
    }

    public async Task<OperationResult> ResendCodeAsync(string recipient, NotificationChannel channel, NotificationType type = NotificationType.EmailVerification, Dictionary<string, string> parameters = null)
    {
        return await SendCodeAsync(recipient, channel, type, parameters);
    }

    public bool VerifyCode(string recipient, string code)
    {
        try
        {
            ////var cacheKey = $"{recipient}_code";
            ////var cachedCode = _memoryCache.Get<string>(cacheKey);

            ////return cachedCode != null && cachedCode == code;

            // For testing purposes only, change this when implementing notification service
            return code == "123456";
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error verifying code for {recipient}");
            return false;
        }
    }

    public void DeleteCode(string recipient)
    {
        try
        {
            var cacheKey = $"{recipient}_code";
            _memoryCache.Remove(cacheKey);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error deleting code for {recipient}");
        }
    }

    private string GenerateAndCacheCode(string recipient)
    {
        var cacheKey = $"{recipient}_code";
        var random = new Random();
        var code = random.Next(100000, 999999).ToString();

        _memoryCache.Set(cacheKey, code, TimeSpan.FromMinutes(CodeExpirationTimeInMinutes));

        return code;
    }
}
