using RestSharp;
using Shared.GeneralModels;
using Shared.GeneralModels.Parameters.Notification;

namespace BL.Contracts.GeneralService.Notification
{
    public interface IEmailProviderService

    {
        ResponseModel<object> Send(EmailRequest request);
        RestResponse SendEmail(string toEmail, string subject, string emailTemplate);
    }
}