using BL.Contracts.GeneralService.Notification;
using Serilog;
using Shared.GeneralModels;
using Shared.GeneralModels.Parameters.Notification;

namespace Bl.GeneralService.Notification;

public class SmsProviderService : ISmsProviderService
{
    private readonly ILogger _logger;

    public SmsProviderService(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<ResponseModel<object>> SendAsync(SmsRequest request)
    {
        try
        {
            // Implement your SMS provider logic here
            // Example with Twilio, AWS SNS, or any other SMS service

            return new ResponseModel<object>
            {
                Success = true,
                Message = "SMS sent successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Failed to send SMS to {request.PhoneNumber}");
            return new ResponseModel<object>
            {
                Success = false,
                Message = "Failed to send SMS"
            };
        }
    }
}
