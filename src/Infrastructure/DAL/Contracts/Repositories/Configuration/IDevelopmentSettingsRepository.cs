using DAL.Contracts.Repositories;
using Domains.Entities.Setting;

namespace DAL.Contracts.Repositories.Configuration;

public interface IDevelopmentSettingsRepository : ITableRepository<TbDevelopmentSettings>
{
    Task<bool> IsMultiVendorModeEnabledAsync();
}