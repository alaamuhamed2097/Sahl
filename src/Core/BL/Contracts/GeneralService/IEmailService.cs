namespace BL.Contracts.GeneralService
{
    public interface IEmailService
    {
        bool SendEmail(string ToEmail, string Subject, string Body, IEnumerable<string> AttachmentFiles = null);
        bool SendEmailConfirmActivation(string ToEmail, string link);
        bool SendEmailConfirmActivationByCode(string ToEmail, string code);
    }
}
