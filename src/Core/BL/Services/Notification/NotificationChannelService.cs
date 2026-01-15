using BL.Contracts.IMapper;
using BL.Contracts.Service.Notification;
using BL.Extensions;
using BL.Services.Base;
using Common.Enumerations.Notification;
using Common.Filters;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.Notification;
using Resources;
using Serilog;
using Shared.DTOs.Notification;
using System.Linq.Expressions;

namespace BL.Services.Notification;

public class NotificationChannelService : BaseService<TbNotificationChannel, NotificationChannelDto>, INotificationChannelService
{
    private readonly ITableRepository<TbNotificationChannel> _channelRepository;
    private readonly ILogger _logger;
    private readonly IBaseMapper _mapper;

    public NotificationChannelService(
        ITableRepository<TbNotificationChannel> channelRepository,
        ILogger logger,
        IBaseMapper mapper)
        : base(channelRepository, mapper)
    {
        _channelRepository = channelRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NotificationChannelDto>> GetAllAsync()
    {
        var channels = await _channelRepository
            .GetAsync(x => !x.IsDeleted);

        return _mapper.MapList<TbNotificationChannel, NotificationChannelDto>(channels).ToList();
    }

    public async Task<IEnumerable<NotificationChannelDto>> GetActiveChannelsAsync()
    {
        var channels = await _channelRepository
            .GetAsync(x => !x.IsDeleted && x.IsActive);

        return _mapper.MapList<TbNotificationChannel, NotificationChannelDto>(channels).ToList();
    }

    public async Task<NotificationChannelDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id));

        var channel = await _channelRepository.FindByIdAsync(id);
        if (channel == null) return null;

        return _mapper.MapModel<TbNotificationChannel, NotificationChannelDto>(channel);
    }

    public async Task<NotificationChannelDto?> GetByChannelTypeAsync(NotificationChannel channelType)
    {
        var channel = (await _channelRepository
            .GetAsync(x => x.Channel == channelType && !x.IsDeleted))
            .FirstOrDefault();

        if (channel == null) return null;

        return _mapper.MapModel<TbNotificationChannel, NotificationChannelDto>(channel);
    }

    public async Task<PagedResult<NotificationChannelDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
    {
        if (criteriaModel == null)
            throw new ArgumentNullException(nameof(criteriaModel));

        if (criteriaModel.PageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

        if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

        Expression<Func<TbNotificationChannel, bool>> filter = x => !x.IsDeleted;

        var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            filter = filter.And(x =>
                x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm) ||
                x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm)
            );
        }

        var channels = await _channelRepository.GetPageAsync(
            criteriaModel.PageNumber,
            criteriaModel.PageSize,
            filter);

        var itemsDto = _mapper.MapList<TbNotificationChannel, NotificationChannelDto>(channels.Items).ToList();

        return new PagedResult<NotificationChannelDto>(itemsDto, channels.TotalRecords);
    }

    public async Task<bool> SaveAsync(NotificationChannelDto dto, Guid userId)
    {
        var entity = _mapper.MapModel<NotificationChannelDto, TbNotificationChannel>(dto);
        var result = await _channelRepository.SaveAsync(entity, userId);
        return result.Success;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        return await _channelRepository.UpdateIsDeletedAsync(id, userId, true);
    }

    public async Task<bool> ToggleActiveStatusAsync(Guid id, Guid userId)
    {
        var channel = await _channelRepository.FindByIdAsync(id);
        if (channel == null)
            throw new KeyNotFoundException($"Notification channel with ID {id} not found");

        channel.IsActive = !channel.IsActive;
        var result = await _channelRepository.UpdateAsync(channel, userId);
        return result.Success;
    }
}
