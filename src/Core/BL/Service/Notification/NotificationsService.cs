using BL.Contracts.IMapper;
using BL.Contracts.Service.Notification;
using BL.Extensions;
using BL.Service.Base;
using Common.Enumerations.Notification;
using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.Notification;
using Resources;
using Serilog;
using Shared.DTOs.Notification;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Service.Notification
{
    public class NotificationsService : BaseService<TbNotifications, NotificationsDto>, INotificationsService
    {
        private readonly ITableRepository<TbNotifications> _notificationsRepository;
        private readonly ILogger _logger;
        private readonly IBaseMapper _mapper;

        public NotificationsService(
            ITableRepository<TbNotifications> notificationsRepository,
            ILogger logger,
            IBaseMapper mapper)
            : base(notificationsRepository, mapper)
        {
            _notificationsRepository = notificationsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationsDto>> GetAllAsync()
        {
            var notifications = await _notificationsRepository
                .GetAsync(x => x.CurrentState == 1, orderBy: q => q.OrderByDescending(x => x.SentDate));

            return _mapper.MapList<TbNotifications, NotificationsDto>(notifications).ToList();
        }

        public async Task<IEnumerable<NotificationsDto>> GetByRecipientAsync(int recipientId, RecipientType recipientType)
        {
            var notifications = await _notificationsRepository
                .GetAsync(x => x.RecipientID == recipientId && x.RecipientType == recipientType && x.CurrentState == 1,
                    orderBy: q => q.OrderByDescending(x => x.SentDate));

            return _mapper.MapList<TbNotifications, NotificationsDto>(notifications).ToList();
        }

        public async Task<IEnumerable<NotificationsDto>> GetUnreadByRecipientAsync(int recipientId, RecipientType recipientType)
        {
            var notifications = await _notificationsRepository
                .GetAsync(x => x.RecipientID == recipientId 
                    && x.RecipientType == recipientType 
                    && !x.IsRead 
                    && x.CurrentState == 1,
                    orderBy: q => q.OrderByDescending(x => x.SentDate));

            return _mapper.MapList<TbNotifications, NotificationsDto>(notifications).ToList();
        }

        public async Task<NotificationsDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var notification = await _notificationsRepository.FindByIdAsync(id);
            if (notification == null) return null;

            return _mapper.MapModel<TbNotifications, NotificationsDto>(notification);
        }

        public async Task<PaginatedDataModel<NotificationsDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            Expression<Func<TbNotifications, bool>> filter = x => x.CurrentState == 1;

            var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filter = filter.And(x =>
                    x.Title != null && x.Title.ToLower().Contains(searchTerm) ||
                    x.Message != null && x.Message.ToLower().Contains(searchTerm)
                );
            }

            var notifications = await _notificationsRepository.GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy: q => q.OrderByDescending(x => x.SentDate));

            var itemsDto = _mapper.MapList<TbNotifications, NotificationsDto>(notifications.Items).ToList();

            return new PaginatedDataModel<NotificationsDto>(itemsDto, notifications.TotalRecords);
        }

        public async Task<bool> SaveAsync(NotificationsDto dto, Guid userId)
        {
            var entity = _mapper.MapModel<NotificationsDto, TbNotifications>(dto);
            var result = await _notificationsRepository.SaveAsync(entity, userId);
            return result.Success;
        }

        public async Task<bool> MarkAsReadAsync(Guid id, Guid userId)
        {
            var notification = await _notificationsRepository.FindByIdAsync(id);
            if (notification == null)
                throw new KeyNotFoundException($"Notification with ID {id} not found");

            notification.IsRead = true;
            notification.ReadDate = DateTime.UtcNow;
            var result = await _notificationsRepository.UpdateAsync(notification, userId);
            return result.Success;
        }

        public async Task<bool> MarkMultipleAsReadAsync(List<Guid> ids, Guid userId)
        {
            if (ids == null || !ids.Any())
                return false;

            foreach (var id in ids)
            {
                var notification = await _notificationsRepository.FindByIdAsync(id);
                if (notification != null)
                {
                    notification.IsRead = true;
                    notification.ReadDate = DateTime.UtcNow;
                    await _notificationsRepository.UpdateAsync(notification, userId);
                }
            }

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            return await _notificationsRepository.UpdateCurrentStateAsync(id, userId, 0);
        }

        public async Task<int> GetUnreadCountAsync(int recipientId, RecipientType recipientType)
        {
            var notifications = await _notificationsRepository
                .GetAsync(x => x.RecipientID == recipientId 
                    && x.RecipientType == recipientType 
                    && !x.IsRead 
                    && x.CurrentState == 1);

            return notifications.Count();
        }
    }
}
