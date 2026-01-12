using BL.Contracts.GeneralService;
using Common.Enumerations.Settings;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Configuration;
using Domains.Entities.Setting;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Configuration;

/// <summary>
/// Repository implementation for system settings with enum support
/// </summary>
public class DevelopmentSettingsRepository : TableRepository<TbDevelopmentSettings>, IDevelopmentSettingsRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DevelopmentSettingsRepository(ApplicationDbContext context, ILogger logger, ICurrentUserService currentUserService) :
        base(context,currentUserService, logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _currentUserService = currentUserService;
    }
    public async Task<bool> IsMultiVendorModeEnabledAsync()
    {
        return await _context.TbDevelopmentSettings
            .AnyAsync(x => !x.IsDeleted && x.IsMultiVendorSystem);
    }

}
