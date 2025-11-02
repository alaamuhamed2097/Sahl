using Shared.DTOs.Currency;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Currency
{
    public interface ICurrencyService
    {
        Task<ResponseModel<IEnumerable<CurrencyDto>>> GetAllAsync();
        Task<ResponseModel<CurrencyDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<CurrencyDto>> SaveAsync(CurrencyDto item);
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
        Task<ResponseModel<bool>> SetBaseCurrencyAsync(Guid currencyId);
        Task<ResponseModel<bool>> UpdateExchangeRatesAsync();
    }
}