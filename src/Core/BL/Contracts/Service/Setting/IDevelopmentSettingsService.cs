using Common.Enumerations.Settings;
using DAL.ResultModels;
using Shared.DTOs.Setting;

namespace BL.Contracts.Service.Setting;

/// <summary>
/// Service interface for development settings management
/// </summary>
public interface IDevelopmentSettingsService
{
    Task<IEnumerable<DevelopmentSettingsDto>> GetAllAsync();
    Task<DevelopmentSettingsDto?> GetByIdAsync(Guid id);
    Task<SaveResult> SaveAsync(DevelopmentSettingsDto dto, Guid userId);
    Task<bool> IsMultiVendorModeEnabledAsync();
    Task<bool> DeleteAsync(Guid id, Guid userId);
}