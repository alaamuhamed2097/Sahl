namespace BL.Contracts.GeneralService.Notification
{
    public interface ISmsService
    {
        Task<bool> SendSmsAsync(string mobileNumber, string message);
    }
}
