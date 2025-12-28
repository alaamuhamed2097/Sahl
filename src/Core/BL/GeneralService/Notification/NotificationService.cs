using BL.Contracts.GeneralService.Notification;
using Common.Enumerations.Notification;
using Resources;
using Resources.Enumerations;
using Serilog;
using Shared.GeneralModels;
using Shared.GeneralModels.Parameters.Notification;

namespace Bl.GeneralService.Notification;

public class NotificationService : INotificationService
{
    private readonly IEmailProviderService _emailProvider;
    private readonly ISmsProviderService _smsProvider;
    private readonly ITemplateService _templateService;
    private readonly ISignalRProviderService _signalRProviderService;
    private readonly IUserNotificationService _userNotificationService;
    private readonly ILogger _logger;

    public NotificationService(
        IEmailProviderService emailProvider,
        ISmsProviderService smsProvider,
        ITemplateService templateService,
        ILogger logger,
        ISignalRProviderService signalRProviderService,
        IUserNotificationService userNotificationService)
    {
        _emailProvider = emailProvider;
        _smsProvider = smsProvider;
        _templateService = templateService;
        _logger = logger;
        _signalRProviderService = signalRProviderService;
        _userNotificationService = userNotificationService;
    }

    public async Task<ResponseModel<object>> SendNotificationAsync(NotificationRequest request)
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
                    return new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Unsupported notification channel"
                    };
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error sending notification to {request.Recipient} via {request.Channel}");

            return new ResponseModel<object>
            {
                Success = false,
                Message = "Failed to send notification"
            };
        }
    }

    public async Task<ResponseModel<object>> SendBulkNotificationAsync(List<NotificationRequest> requests)
    {
        var results = new List<ResponseModel<object>>();
        var successCount = 0;

        foreach (var request in requests)
        {
            var result = await SendNotificationAsync(request);
            results.Add(result);
            if (result.Success) successCount++;
        }

        return new ResponseModel<object>
        {
            Success = successCount == requests.Count,
            Message = $"Sent {successCount} out of {requests.Count} notifications successfully"
        };
    }

    private async Task<ResponseModel<object>> SendEmailNotificationAsync(NotificationRequest request)
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

        return _emailProvider.Send(emailRequest);
    }

    private async Task<ResponseModel<object>> SendSmsNotificationAsync(NotificationRequest request)
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

    private async Task<ResponseModel<object>> SendSignalRNotificationAsync(NotificationRequest request)
    {
        var templateAr = await _templateService.GetTemplateAsync(
                        request.CustomTemplate, request.Channel, request.Type);
        var bodyAr = _templateService.ProcessTemplate(templateAr.Content, request.Parameters);
        var titleAr = !string.IsNullOrEmpty(request.Title) ? request.Title : templateAr.Title;
        titleAr = _templateService.ProcessTemplate(titleAr, request.Parameters);

        var templateEn = await _templateService.GetTemplateAsync(
                        request.CustomTemplate, request.Channel, request.Type, "en");
        var bodyEn = _templateService.ProcessTemplate(templateEn.Content, request.Parameters);
        var titleEn = !string.IsNullOrEmpty(request.Title) ? request.Title : templateEn.Title;
        titleEn = _templateService.ProcessTemplate(titleEn, request.Parameters);

        var template = ResourceManager.CurrentLanguage == Language.Arabic ? templateAr : templateEn;
        var body = ResourceManager.CurrentLanguage == Language.Arabic ? bodyAr : bodyEn;
        var title = ResourceManager.CurrentLanguage == Language.Arabic ? titleAr : titleEn;

        var signalRRequest = new SignalRNotificationRequest
        {
            Title = title,
            Message = body,
            UserId = request.Recipient
        };

        // Save notification
        var IsSaved = await _userNotificationService.Save(new UserNotificationRequest
        {
            TitleAr = titleAr,
            TitleEn = titleEn,
            DescriptionAr = bodyAr,
            DescriptionEn = bodyEn,
            UserId = request.Recipient
        }, Guid.Parse(request.Recipient));

        return await _signalRProviderService.SendNotificationToUser(signalRRequest);
    }
}
