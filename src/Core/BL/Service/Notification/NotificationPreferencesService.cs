using BL.Contracts.IMapper;
using BL.Contracts.Service.Notification;
using BL.Service.Base;
using Common.Enumerations.Notification;
using DAL.Contracts.Repositories;
using Domains.Entities.Notification;
using Serilog;
using Shared.DTOs.Notification;

namespace BL.Service.Notification
{
    public class NotificationPreferencesService : BaseService<TbNotificationPreferences, NotificationPreferencesDto>, INotificationPreferencesService
    {
        private readonly ITableRepository<TbNotificationPreferences> _preferencesRepository;
        private readonly ILogger _logger;
        private readonly IBaseMapper _mapper;

        public NotificationPreferencesService(
            ITableRepository<TbNotificationPreferences> preferencesRepository,
            ILogger logger,
            IBaseMapper mapper)
            : base(preferencesRepository, mapper)
        {
            _preferencesRepository = preferencesRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationPreferencesDto>> GetByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            var preferences = await _preferencesRepository
                .GetAsync(x => x.UserId == userId && !x.IsDeleted);

            return _mapper.MapList<TbNotificationPreferences, NotificationPreferencesDto>(preferences).ToList();
        }

        public async Task<NotificationPreferencesDto?> GetByUserAndTypeAsync(string userId, NotificationType notificationType)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            var preference = (await _preferencesRepository
                .GetAsync(x => x.UserId == userId && x.NotificationType == notificationType && !x.IsDeleted))
                .FirstOrDefault();

            if (preference == null) return null;

            return _mapper.MapModel<TbNotificationPreferences, NotificationPreferencesDto>(preference);
        }

        public async Task<NotificationPreferencesDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var preference = await _preferencesRepository.FindByIdAsync(id);
            if (preference == null) return null;

            return _mapper.MapModel<TbNotificationPreferences, NotificationPreferencesDto>(preference);
        }

        public async Task<bool> SaveAsync(NotificationPreferencesDto dto, Guid userId)
        {
            // Check for existing preference
            var existing = (await _preferencesRepository
                .GetAsync(x => x.UserId == dto.UserId
                    && x.UserType == dto.UserType
                    && x.NotificationType == dto.NotificationType
                    && x.Id != dto.Id
                    && !x.IsDeleted))
                .FirstOrDefault();

            if (existing != null)
                throw new InvalidOperationException("Preference already exists for this user and notification type");

            var entity = _mapper.MapModel<NotificationPreferencesDto, TbNotificationPreferences>(dto);
            var result = await _preferencesRepository.SaveAsync(entity, userId);
            return result.Success;
        }

        public async Task<bool> SaveRangeAsync(IEnumerable<NotificationPreferencesDto> dtos, Guid userId)
        {
            var entities = _mapper.MapList<NotificationPreferencesDto, TbNotificationPreferences>(dtos).ToList();
            return await _preferencesRepository.AddRangeAsync(entities, userId);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            return await _preferencesRepository.UpdateCurrentStateAsync(id, userId, true);
        }

        public async Task<bool> UpdatePreferenceAsync(
            string userId,
            NotificationType notificationType,
            bool enableEmail,
            bool enableSMS,
            bool enablePush,
            bool enableInApp,
            Guid updaterId)
        {
            var preference = (await _preferencesRepository
                .GetAsync(x => x.UserId == userId && x.NotificationType == notificationType && !x.IsDeleted))
                .FirstOrDefault();

            if (preference == null)
                throw new KeyNotFoundException($"Preference not found for user {userId} and notification type {notificationType}");

            preference.EnableEmail = enableEmail;
            preference.EnableSMS = enableSMS;
            preference.EnablePush = enablePush;
            preference.EnableInApp = enableInApp;

            var result = await _preferencesRepository.UpdateAsync(preference, updaterId);
            return result.Success;
        }
    }
}
