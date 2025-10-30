using Shared.DTOs.Currency;
using Shared.GeneralModels;

namespace BL.Contracts.Service.Currency
{
    public interface ICurrencyService
    {
        Task<ResponseModel<IEnumerable<CurrencyDto>>> GetAllAsync();
        Task<ResponseModel<CurrencyDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<CurrencyDto>> SaveAsync(CurrencyDto dto, Guid userId);
        Task<ResponseModel<bool>> DeleteAsync(Guid currencyId, Guid userId);
        Task<CurrencyDto> GetBaseCurrencyAsync();
        Task<IEnumerable<CurrencyDto>> GetActiveCurrenciesAsync();
        Task<bool> UpdateExchangeRatesAsync(Guid userId);
        Task<bool> SetBaseCurrencyAsync(Guid currencyId, Guid userId);
    }
}