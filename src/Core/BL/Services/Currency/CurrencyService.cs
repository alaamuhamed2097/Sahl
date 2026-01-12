using BL.Contracts.IMapper;
using BL.Contracts.Service.Currency;
using BL.Services.Base;
using DAL.Contracts.Repositories;
using DAL.ResultModels;
using Domains.Entities.Currency;
using Resources;
using Shared.DTOs.Currency;
using Shared.GeneralModels;

namespace BL.Services.Currency;

public class CurrencyService : BaseService<TbCurrency, CurrencyDto>, ICurrencyService
{
    private readonly ITableRepository<TbCurrency> _currencyRepository;
    private readonly IBaseMapper _mapper;

    public CurrencyService(ITableRepository<TbCurrency> currencyRepository, IBaseMapper mapper)
        : base(currencyRepository, mapper)
    {
        _currencyRepository = currencyRepository;
        _mapper = mapper;
    }

    public async Task<ResponseModel<IEnumerable<CurrencyDto>>> GetAllAsync()
    {
        try
        {
            var entities = await _currencyRepository.GetAllAsync();
            var dtos = _mapper.MapList<TbCurrency, CurrencyDto>(entities);

            return new ResponseModel<IEnumerable<CurrencyDto>>
            {
                Success = true,
                Data = dtos
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel<IEnumerable<CurrencyDto>>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<ResponseModel<CurrencyDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var entity = await _currencyRepository.FindAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
            {
                return new ResponseModel<CurrencyDto>
                {
                    Success = false,
                    Message = string.Format(ValidationResources.EntityNotFound, "Currency")
                };
            }

            var dto = _mapper.MapModel<TbCurrency, CurrencyDto>(entity);
            return new ResponseModel<CurrencyDto>
            {
                Success = true,
                Data = dto
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel<CurrencyDto>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<ResponseModel<CurrencyDto>> SaveAsync(CurrencyDto dto, Guid userId)
    {
        try
        {
            // Validate input
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (userId == Guid.Empty) throw new ArgumentException("UserId cannot be empty", nameof(userId));

            var entity = _mapper.MapModel<CurrencyDto, TbCurrency>(dto);
            SaveResult result;

            // Create new currency
            result = await _currencyRepository.SaveAsync(entity, userId);
            if (!result.Success)
            {
                throw new Exception(NotifiAndAlertsResources.SaveFailed);
            }
            dto.Id = result.Id;


            // If this is set as base currency, ensure only one base currency exists
            if (dto.IsBaseCurrency)
            {
                await EnsureSingleBaseCurrencyAsync(dto.Id, userId);
            }

            return new ResponseModel<CurrencyDto>
            {
                Success = true,
                Data = dto,
                Message = NotifiAndAlertsResources.SavedSuccessfully
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel<CurrencyDto>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    // Fixed to match interface signature (Guid currencyId instead of CurrencyDto dto)
    public async Task<ResponseModel<bool>> DeleteAsync(Guid currencyId, Guid userId)
    {
        try
        {
            // Validate input
            if (currencyId == Guid.Empty) throw new ArgumentException("CurrencyId cannot be empty", nameof(currencyId));
            if (userId == Guid.Empty) throw new ArgumentException("UserId cannot be empty", nameof(userId));

            // First get the currency to check if it's a base currency
            var currency = await _currencyRepository.FindAsync(x => x.Id == currencyId && !x.IsDeleted);
            if (currency == null)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = string.Format(ValidationResources.EntityNotFound, "Currency")
                };
            }

            if (currency.IsBaseCurrency)
            {
                return new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Cannot delete base currency"
                };
            }

            var success = await _currencyRepository.UpdateIsDeletedAsync(currencyId, userId, true);
            if (!success)
            {
                throw new Exception(NotifiAndAlertsResources.DeleteFailed);
            }

            return new ResponseModel<bool>
            {
                Success = true,
                Data = true,
                Message = NotifiAndAlertsResources.DeletedSuccessfully
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel<bool>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    public async Task<CurrencyDto> GetBaseCurrencyAsync()
    {
        var baseCurrency = await _currencyRepository.FindAsync(x => x.IsBaseCurrency && !x.IsDeleted);

        if (baseCurrency == null)
        {
            // Return USD as default if no base currency is set
            baseCurrency = await _currencyRepository.FindAsync(x => x.Code == "USD" && !x.IsDeleted);
        }

        return _mapper.MapModel<TbCurrency, CurrencyDto>(baseCurrency);
    }

    public async Task<IEnumerable<CurrencyDto>> GetActiveCurrenciesAsync()
    {
        var currencies = await _currencyRepository.GetAsync(x => x.IsActive && !x.IsDeleted);
        return _mapper.MapList<TbCurrency, CurrencyDto>(currencies);
    }

    public async Task<bool> UpdateExchangeRatesAsync(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty) throw new ArgumentException("UserId cannot be empty", nameof(userId));

            // This would typically integrate with an external API like ExchangeRate-API
            // For now, we'll update the LastUpdated timestamp
            var currencies = await _currencyRepository.GetAsync(x => !x.IsDeleted);

            foreach (var currency in currencies)
            {
                currency.UpdatedDateUtc = DateTime.UtcNow;
                await _currencyRepository.UpdateAsync(currency, userId);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Sets a specific currency as the base currency, ensuring only one base currency exists
    /// </summary>
    /// <param name="currencyId">The ID of the currency to set as base</param>
    /// <param name="userId">The ID of the user making the change</param>
    /// <returns>True if successful, false otherwise</returns>
    public async Task<bool> SetBaseCurrencyAsync(Guid currencyId, Guid userId)
    {
        try
        {
            // Validate input parameters
            if (currencyId == Guid.Empty)
                throw new ArgumentException("CurrencyId cannot be empty", nameof(currencyId));
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty", nameof(userId));

            // Check if the target currency exists and is active
            var targetCurrency = await _currencyRepository.FindAsync(x => x.Id == currencyId && !x.IsDeleted);
            if (targetCurrency == null)
            {
                return false; // Currency not found or inactive
            }

            // Check if it's already the base currency
            if (targetCurrency.IsBaseCurrency)
            {
                return true; // Already base currency, no action needed
            }

            // Step 1: Remove base currency flag from all currencies
            var allCurrencies = await _currencyRepository.GetAsync(x => !x.IsDeleted);

            foreach (var currency in allCurrencies.Where(c => c.IsBaseCurrency))
            {
                currency.IsBaseCurrency = false;
                var updateResult = await _currencyRepository.UpdateAsync(currency, userId);
                if (!updateResult.Success)
                {
                    return false; // Failed to update existing base currency
                }
            }

            // Step 2: Set the new base currency
            targetCurrency.IsBaseCurrency = true;
            targetCurrency.ExchangeRate = 1.0m; // Base currency always has exchange rate of 1
            targetCurrency.IsActive = true; // Ensure base currency is active
            targetCurrency.UpdatedDateUtc = DateTime.UtcNow;

            var result = await _currencyRepository.UpdateAsync(targetCurrency, userId);

            return result.Success;
        }
        catch (Exception)
        {
            // Log the exception here if you have logging
            // _logger.LogError(ex, "Error setting base currency {CurrencyId}", currencyId);
            return false;
        }
    }

    #region Private Helper Methods

    /// <summary>
    /// Ensures only one currency can be marked as base currency
    /// This is used during SaveAsync operations
    /// </summary>
    private async Task EnsureSingleBaseCurrencyAsync(Guid newBaseCurrencyId, Guid userId)
    {
        // Remove base currency flag from all other currencies
        var allCurrencies = await _currencyRepository.GetAsync(x => !x.IsDeleted);
        foreach (var currency in allCurrencies)
        {
            if (currency.IsBaseCurrency && currency.Id != newBaseCurrencyId)
            {
                currency.IsBaseCurrency = false;
                await _currencyRepository.UpdateAsync(currency, userId);
            }
        }

        // Set the new base currency properties
        var newBaseCurrency = await _currencyRepository.FindAsync(x => x.Id == newBaseCurrencyId && !x.IsDeleted);
        if (newBaseCurrency != null)
        {
            newBaseCurrency.IsBaseCurrency = true;
            newBaseCurrency.ExchangeRate = 1m; // Base currency always has rate of 1
            newBaseCurrency.UpdatedDateUtc = DateTime.UtcNow;
            await _currencyRepository.UpdateAsync(newBaseCurrency, userId);
        }
    }

    #endregion
}