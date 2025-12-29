using Bl.Contracts.GeneralService.Notification;
using Serilog;
using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.ResultModels;

namespace Bl.GeneralService.Notification
{
    public class SmsProviderService : ISmsProviderService
    {
        private readonly ILogger _logger;

        public SmsProviderService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<OperationResult> SendAsync(SmsRequest request)
        {
            try
            {
                // Implement your SMS provider logic here
                // Example with Twilio, AWS SNS, or any other SMS service

                return new OperationResult
                {
                    Success = true,
                    Message = "SMS sent successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Failed to send SMS to {request.PhoneNumber}");
                return new OperationResult
                {
                    Success = false,
                    Message = "Failed to send SMS"
                };
            }
        }
    }
}
