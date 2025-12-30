using Shared.GeneralModels.Models;

namespace Bl.Contracts.GeneralService.Notification
{
    public interface IInvoiceINotificationService
    {
        Task<bool> SendInvoiceNotificationAsync(string invoiceId, CountryPricingInfo countryPricing, string studentId);
    }
}
