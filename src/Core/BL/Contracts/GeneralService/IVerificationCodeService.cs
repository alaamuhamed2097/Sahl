using Common.Enumerations.Notification;
using Shared.GeneralModels.ResultModels;

namespace BL.Contracts.GeneralService
{
    public interface IVerificationCodeService
    {
        Task<OperationResult> SendCodeAsync(string recipient, NotificationChannel channel, NotificationType type = NotificationType.EmailVerification, Dictionary<string, string> parameters = null);
        bool VerifyCode(string recipient, string code);
        void DeleteCode(string recipient);
        Task<OperationResult> ResendCodeAsync(string recipient, NotificationChannel channel, NotificationType type = NotificationType.EmailVerification, Dictionary<string, string> parameters = null);
    }
}