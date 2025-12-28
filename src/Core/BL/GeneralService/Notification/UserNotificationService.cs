using BL.Contracts.GeneralService.Notification;
using BL.Contracts.IMapper;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Notification;
using Domains.Views.UserNotification;
using Serilog;
using Shared.DTOs.Notification;

namespace BL.GeneralService.Notification
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseMapper _mapper;
        private readonly ILogger _logger;

        public UserNotificationService(
            IUnitOfWork unitOfWork,
            IBaseMapper mapper,
            ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        //public PaginatedDataModel<NotificationDto> GetPage(string userId, int page = 1, int pageSize = 10)
        //{
        //    if (string.IsNullOrEmpty(userId))
        //        throw new ArgumentNullException(nameof(userId));

        //    if (page < 1)
        //        throw new ArgumentOutOfRangeException(nameof(page), "Page number must be greater than zero.");

        //    var paginatedData = _unitOfWork.Repository<VwUserNotifications>().GetPage(
        //        page,
        //        pageSize,
        //        n => n.UserId == userId,
        //        orderBy: q => q.OrderByDescending(n => n.CreatedDate));

        //    var unReadCount = GetUnreadNotificationCount(userId);
        //    var dtoList = _mapper.MapList<VwUserNotifications, NotificationDto>(paginatedData.Data.ToList());

        //    return new PaginatedDataModel<NotificationDto>(dtoList, paginatedData.TotalCount);
        //}

        public async Task<IEnumerable<NotificationDto>> GetAllAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            const int maxItems = 10;
            var userNotifications = (await _unitOfWork.Repository<VwUserNotification>()
                .GetAsync(n => n.UserId == userId))
                .OrderByDescending(n => n.CreatedDateUtc)
                .Take(maxItems)
                .ToList();

            var unReadCount = GetUnreadNotificationCount(userId);

            return _mapper.MapList<VwUserNotification, NotificationDto>(userNotifications);
        }

        public async Task<NotificationDto> FindByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Notification ID cannot be empty.", nameof(id));

            var userNotification = await _unitOfWork.Repository<VwUserNotification>()
                .FindAsync(n => n.Id == id);

            if (userNotification == null)
                throw new KeyNotFoundException("Notification not found.");

            return _mapper.MapModel<VwUserNotification, NotificationDto>(userNotification);
        }

        public async Task<bool> Save(NotificationDto dto, Guid userId)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var notification = new TbNotification
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    //Description = "",
                    //Url = dto.Url,
                    //ImagePath = dto.ImagePath,
                    //SenderType = dto.SenderType
                };

                var isSaved = await _unitOfWork.TableRepository<TbNotification>()
                    .SaveAsync(notification, userId);

                if (!isSaved.Success)
                    throw new Exception("Failed to save notification.");

                // Create new user notification
                var userNotificationEntity = new TbUserNotification
                {
                    //UserNotificationId = notificationId,
                    UserId = dto.UserId,
                    IsRead = dto.IsRead,
                    //CurrentState = 1
                };

                isSaved = await _unitOfWork.TableRepository<TbUserNotification>()
                    .CreateAsync(userNotificationEntity, userId);

                if (!isSaved.Success)
                    throw new Exception("Failed to save user notification.");

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.Error(ex, "Error saving notification");
                throw;
            }
        }

        public async Task<bool> SaveBulk(NotificationDto dto, IEnumerable<string> recipients)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (recipients == null || !recipients.Any())
                throw new ArgumentNullException(nameof(recipients));

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Create the main notification
                var notification = new TbNotification
                {
                    Title = dto.Title,
                    //Description = "",
                    //Url = dto.Url,
                    //ImagePath = dto.ImagePath,
                    //SenderType = dto.SenderType
                };

                var isSaved = await _unitOfWork.TableRepository<TbNotification>()
                    .SaveAsync(notification, Guid.Empty);

                if (!isSaved.Success)
                    throw new Exception("Failed to save notification.");

                // Prepare user notifications for bulk creation
                var userNotifications = recipients.Select(userId => new TbUserNotification
                {
                    //Id = notificationId,
                    //UserId = userId,
                    //IsRead = false,  // Default to unread
                    //CurrentState = 1
                }).ToList();

                // Create user notifications in bulk
                var bulkResult = await _unitOfWork.TableRepository<TbUserNotification>()
                    .AddRangeAsync(userNotifications, Guid.Empty);

                if (!bulkResult)
                    throw new Exception("Failed to save user notifications in bulk.");

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.Error(ex, "Error saving bulk notifications");
                throw;
            }
        }

        public async Task<bool> MarkAsRead(List<Guid> NotificationIds, string userId)
        {
            if (NotificationIds == null || !NotificationIds.Any())
                throw new ArgumentNullException(nameof(NotificationIds));

            if (!Guid.TryParse(userId, out var parsedUserId) || parsedUserId == Guid.Empty)
                throw new ArgumentException("Invalid user ID.", nameof(userId));

            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var userNotifications = (await _unitOfWork.TableRepository<TbUserNotification>()
                    .GetAsync(un => NotificationIds.Contains(un.Id) && un.UserId == userId))
                    .ToList();

                foreach (var userNotification in userNotifications)
                {
                    userNotification.IsRead = true;
                    await _unitOfWork.TableRepository<TbUserNotification>()
                        .UpdateAsync(userNotification, parsedUserId);
                }

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.Error(ex, "Error marking notifications as read");
                throw;
            }
        }

        public async Task<bool> Delete(Guid id, Guid userId)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Notification ID cannot be empty.", nameof(id));

            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));

            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Soft delete the notification
                await _unitOfWork.TableRepository<TbNotification>().UpdateCurrentStateAsync(id, userId);

                // Soft delete associated user notifications
                var userNotifications = await _unitOfWork.TableRepository<TbUserNotification>()
                    .GetAsync(x => x.Id == id);

                foreach (var userNotification in userNotifications)
                {
                    await _unitOfWork.TableRepository<TbUserNotification>()
                        .UpdateCurrentStateAsync(userNotification.Id, userId);
                }

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.Error(ex, "Error deleting notification");
                throw;
            }
        }

        private int GetUnreadNotificationCount(string userId)
        {
            //return _unitOfWork.TableRepository<TbUserNotification>()
            //    .Count(n => n.UserId == userId && n.IsRead == false);

            return 0;
        }
    }
}
