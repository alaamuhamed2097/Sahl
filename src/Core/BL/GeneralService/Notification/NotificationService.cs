using Bl.Contracts.GeneralService.Notification;
using BL.Contracts.GeneralService.Notification;
using BL.OperationResults;
using Common.Enumerations.Notification;
using Serilog;
using Shared.DTOs.Notification;
using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.ResultModels;

namespace Bl.GeneralService.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailProviderService _emailProvider;
        private readonly ISmsProviderService _smsProvider;
        private readonly ISignalRNotificationService _signalRProvider;
        private readonly ITemplateService _templateService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly ILogger _logger;

        public NotificationService(
            IEmailProviderService emailProvider,
            ISmsProviderService smsProvider,
            ISignalRNotificationService signalRProvider,
            ITemplateService templateService,
            IUserNotificationService userNotificationService,
            ILogger logger)
        {
            _emailProvider = emailProvider;
            _smsProvider = smsProvider;
            _signalRProvider = signalRProvider;
            _templateService = templateService;
            _userNotificationService = userNotificationService;
            _logger = logger;
        }

        public async Task<OperationResult> SendNotificationAsync(NotificationRequest request)
        {
            try
            {
                switch (request.Channel)
                {
                    case NotificationChannel.Email:
                        return await SendEmailNotificationAsync(request);

                    case NotificationChannel.Sms:
                        return await SendSmsNotificationAsync(request);

                    case NotificationChannel.SignalR:
                        return await SendSignalRNotificationAsync(request);

                    default:
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Unsupported notification channel"
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error sending notification to {request.Recipient} via {request.Channel}");

                return new OperationResult
                {
                    Success = false,
                    Message = "Failed to send notification"
                };
            }
        }

        public async Task<BulkOperationResult> SendBulkNotificationAsync(NotificationRequest request, IEnumerable<string> recipients)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var recipientList = recipients?.ToList() ?? new List<string>();
            if (!recipientList.Any())
            {
                return new BulkOperationResult
                {
                    Success = false,
                    Message = "No recipients provided",
                    TotalCount = 0,
                    SuccessCount = 0
                };
            }

            try
            {
                switch (request.Channel)
                {
                    case NotificationChannel.SignalR:
                        return await SendBulkSignalRNotificationAsync(request, recipientList);

                    case NotificationChannel.Email:
                        return await SendBulkEmailNotificationAsync(request, recipientList);

                    case NotificationChannel.Sms:
                        return await SendBulkSmsNotificationAsync(request, recipientList);

                    default:
                        return new BulkOperationResult
                        {
                            Success = false,
                            Message = "Unsupported bulk notification channel",
                            TotalCount = recipientList.Count,
                            SuccessCount = 0
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error sending bulk notifications");
                return new BulkOperationResult
                {
                    Success = false,
                    Message = ex.Message,
                    TotalCount = recipientList.Count,
                    SuccessCount = 0
                };
            }
        }


        private async Task<BulkOperationResult> SendBulkSignalRNotificationAsync(NotificationRequest request, List<string> recipients)
        {
            var template = await _templateService.GetTemplateAsync(
                request.CustomTemplate, request.Channel, request.Type);
            var notificationContent = _templateService.ProcessTemplate(template.Content, request.Parameters);

            // Save notifications in bulk
            var notificationDto = new NotificationDto
            {
                Title = notificationContent,
                UserId = request.Recipient,
                ImagePath = request.ImagePath,
                Url = request.CallToActionUrl
            };

            await _userNotificationService.SaveBulk(notificationDto, recipients);

            // Send SignalR notifications in bulk
            var signalRRequest = new SignalRNotificationRequest
            {
                Title = notificationContent,
                Message = notificationContent,
                UserIds = recipients
            };

            var signalRResult = await _signalRProvider.SendNotificationToUsers(signalRRequest);

            return new BulkOperationResult
            {
                Success = signalRResult.Success,
                Message = signalRResult.Message,
                TotalCount = recipients.Count,
                SuccessCount = signalRResult.Success ? recipients.Count : 0,
                IndividualResults = recipients.Select(r => new OperationResult
                {
                    Success = signalRResult.Success,
                    Message = signalRResult.Message
                }).ToList()
            };
        }

        private async Task<BulkOperationResult> SendBulkEmailNotificationAsync(NotificationRequest request, List<string> recipients)
        {
            var template = await _templateService.GetTemplateAsync(
                request.CustomTemplate, request.Channel, request.Type);

            var content = _templateService.ProcessTemplate(template.Content, request.Parameters);
            var subject = !string.IsNullOrEmpty(request.Subject) ? request.Subject : template.Subject;
            subject = _templateService.ProcessTemplate(subject, request.Parameters);

            var tasks = recipients.Select(recipient =>
                _emailProvider.SendAsync(new EmailRequest(recipient, subject, content)));

            var results = await Task.WhenAll(tasks);

            return new BulkOperationResult
            {
                Success = results.All(r => r.Success),
                Message = $"Sent {results.Count(r => r.Success)} of {recipients.Count} emails",
                TotalCount = recipients.Count,
                SuccessCount = results.Count(r => r.Success),
                IndividualResults = results.ToList()
            };
        }

        private async Task<BulkOperationResult> SendBulkSmsNotificationAsync(NotificationRequest request, List<string> recipients)
        {
            var template = await _templateService.GetTemplateAsync(
                request.CustomTemplate, request.Channel, request.Type);

            var message = _templateService.ProcessTemplate(template.Content, request.Parameters);

            var tasks = recipients.Select(recipient =>
                _smsProvider.SendAsync(new SmsRequest
                {
                    PhoneNumber = recipient,
                    Message = message
                }))
                .ToList();

            var results = await Task.WhenAll(tasks);

            return new BulkOperationResult
            {
                Success = results.All(r => r.Success),
                Message = $"Sent {results.Count(r => r.Success)} of {recipients.Count} SMS",
                TotalCount = recipients.Count,
                SuccessCount = results.Count(r => r.Success),
                IndividualResults = results.ToList()
            };
        }


        private async Task<OperationResult> SendEmailNotificationAsync(NotificationRequest request)
        {
            var template = await _templateService.GetTemplateAsync(
                request.CustomTemplate, request.Channel, request.Type);

            var content = _templateService.ProcessTemplate(template.Content, request.Parameters);
            var subject = !string.IsNullOrEmpty(request.Subject) ? request.Subject : template.Subject;
            subject = _templateService.ProcessTemplate(subject, request.Parameters);

            var emailRequest = new EmailRequest(
                request.Recipient,
                subject,
                content);

            return await _emailProvider.SendAsync(emailRequest);
        }

        private async Task<OperationResult> SendSmsNotificationAsync(NotificationRequest request)
        {
            var template = await _templateService.GetTemplateAsync(
                request.CustomTemplate, request.Channel, request.Type);

            var message = _templateService.ProcessTemplate(template.Content, request.Parameters);

            var smsRequest = new SmsRequest
            {
                PhoneNumber = request.Recipient,
                Message = message
            };

            return await _smsProvider.SendAsync(smsRequest);
        }

        private async Task<OperationResult> SendSignalRNotificationAsync(NotificationRequest request)
        {
            var template = await _templateService.GetTemplateAsync(
                            request.CustomTemplate, request.Channel, request.Type);
            var notification = _templateService.ProcessTemplate(template.Content, request.Parameters);

            var signalRRequest = new SignalRNotificationRequest
            {
                Title = notification,
                Message = notification,
                UserId = request.Recipient
            };

            // Save notification
            var IsSaved = await _userNotificationService.Save(new NotificationDto
            {
                Title = notification,
                UserId = request.Recipient,
                //ImagePath = request.ImagePath,
                Url = request.CallToActionUrl,
            }, Guid.Parse(request.Recipient));

            return await _signalRProvider.SendNotificationToUser(signalRRequest);
        }
    }
}
