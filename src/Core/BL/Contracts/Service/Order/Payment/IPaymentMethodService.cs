using Shared.DTOs.Order.Payment;

namespace BL.Contracts.Service.Order.Payment
{
    public interface IPaymentMethodService
    {
        /// <summary>
        /// Get all available payment methods
        /// </summary>
        Task<List<PaymentMethodDto>> GetAllPaymentMethodsAsync();

        /// <summary>
        /// Get active payment methods only
        /// </summary>
        Task<List<PaymentMethodDto>> GetActivePaymentMethodsAsync();

        /// <summary>
        /// Get payment method by ID
        /// </summary>
        Task<PaymentMethodDto?> GetPaymentMethodByIdAsync(Guid id);
    }
}
