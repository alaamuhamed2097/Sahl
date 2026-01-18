using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Service.Setting;
using BL.Extensions;
using Common.Enumerations.Settings;
using Common.Filters;
using DAL.Contracts.Repositories.Configuration;
using DAL.Models;
using DAL.ResultModels;
using Domains.Entities.Catalog.Brand;
using Domains.Entities.Setting;
using Microsoft.AspNetCore.Hosting;
using Resources;
using Serilog;
using Shared.DTOs.Brand;
using Shared.DTOs.Setting;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace BL.Services.Setting;

/// <summary>
/// Service for managing system settings with caching support
/// </summary>
public class DevelopmentSettingsService : IDevelopmentSettingsService
{
    private readonly IDevelopmentSettingsRepository _developmentSettingsRepository;
    private readonly ILogger _logger;
    private readonly IBaseMapper _mapper;

    public DevelopmentSettingsService(
        ILogger logger,
        IDevelopmentSettingsRepository developmentSettingsRepository,
        IBaseMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _developmentSettingsRepository = developmentSettingsRepository;
        _mapper = mapper;
    }

    public async Task<DevelopmentSettingsDto> GetAsync()
    {
        var brands = (await _developmentSettingsRepository
            .GetAsync(x => !x.IsDeleted)).FirstOrDefault();

        var brandDto = _mapper.MapModel<TbDevelopmentSettings, DevelopmentSettingsDto>(brands);

        return brandDto;
    }
    public async Task<bool> IsMultiVendorModeEnabledAsync()
    {
        return await _developmentSettingsRepository
            .IsMultiVendorModeEnabledAsync();
    }

    public async Task<DevelopmentSettingsDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id));

        var brand = await _developmentSettingsRepository
            .FindByIdAsync(id);

        if (brand == null) return null;

        var brandDto = _mapper.MapModel<TbDevelopmentSettings, DevelopmentSettingsDto>(brand);

        return brandDto;
    }

    public async Task<SaveResult> SaveAsync(DevelopmentSettingsDto dto, Guid userId)
    {
        var entity = _mapper.MapModel<DevelopmentSettingsDto, TbDevelopmentSettings>(dto);
        return await _developmentSettingsRepository.SaveAsync(entity, userId);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var success = await _developmentSettingsRepository
            .UpdateIsDeletedAsync(id, userId, true);

        return success;
    }
}