using BL.Contracts.GeneralService.Notification;

namespace BL.GeneralService.Notification
{
    public class SmsService : ISmsService
    {

        public async Task<bool> SendSmsAsync(string to, string message)
        {
            return true;
            //try
            //{
            //    var messageResult = await MessageResource.CreateAsync(
            //        to: new Twilio.Types.PhoneNumber(to),
            //        from: new Twilio.Types.PhoneNumber(_smsSettings.FromNumber),
            //        body: message
            //    );

            //    // Check if the message was sent successfully
            //    return messageResult.Status != MessageResource.StatusEnum.Failed;
            //}
            //catch (Exception ex)
            //{
            //    // Log exception (implement logging if necessary)
            //    Console.WriteLine($"Error sending SMS: {ex.Message}");
            //    return false;
            //}
        }
    }
}
