using BL.Contracts.GeneralService.Notification;
using BL.Contracts.IMapper;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using Domains.Entities.Notification;
using Domains.Views.UserNotification;
using Resources;
using Serilog;
using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.SearchCriteriaModels;
using Shared.ResultModels;
using System.Linq.Expressions;

namespace BL.GeneralService.Notification
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IUnitOfWork _userNotificationUnitOfWork;
        private readonly IBaseMapper _mapper;
        private readonly ILogger _logger;
        public UserNotificationService(IUnitOfWork userNotificationUnitOfWork, IBaseMapper mapper, ILogger logger)
        {
            _userNotificationUnitOfWork = userNotificationUnitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<UserNotificationResult<PagedResult<UserNotificationRequest>>> GetPage(BaseSearchCriteriaModel criteriaModel, string userId)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter for active entities
            Expression<Func<VwUserNotification, bool>> filter = x => x.UserId == userId;

            // Apply search term if provided
            if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
            {
                string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
                filter = x => (x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm)) ||
                             (x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm)) ||
                             (x.DescriptionAr != null && x.DescriptionAr.ToLower().Contains(searchTerm)) ||
                             (x.DescriptionEn != null && x.DescriptionEn.ToLower().Contains(searchTerm));
            }

            var entitiesList = await _userNotificationUnitOfWork.Repository<VwUserNotification>().GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter, orderBy: q => q.OrderByDescending(n => n.CreatedDateUtc));
            var userNotifications = await _userNotificationUnitOfWork.TableRepository<TbUserNotification>().GetAsync(n => n.UserId == userId);
            var unReadCount = userNotifications.Count(n => n.IsRead == false);
            var totalCount = userNotifications.Count();
            var dtoList = _mapper.MapList<VwUserNotification, UserNotificationRequest>(entitiesList.Items);

            return new UserNotificationResult<PagedResult<UserNotificationRequest>>()
            {
                Value = new PagedResult<UserNotificationRequest>(dtoList, entitiesList.TotalRecords),
                UnReadCount = unReadCount,
                TotalCount = totalCount
            };
        }
        public async Task<UserNotificationResult<IEnumerable<UserNotificationRequest>>> GetAll(string userId)
        {
            var allUserNotifications = await _userNotificationUnitOfWork.Repository<VwUserNotification>().GetAsync(n => n.UserId == userId);
            var userNotifications = allUserNotifications.OrderByDescending(n => n.CreatedDateUtc).Take(10);
            var unReadCount = allUserNotifications.Count(n => n.IsRead == false);
            var totalCount = allUserNotifications.Count();
            return new UserNotificationResult<IEnumerable<UserNotificationRequest>>()
            {
                Value = _mapper.MapList<VwUserNotification, UserNotificationRequest>(userNotifications),
                UnReadCount = unReadCount,
                TotalCount = totalCount
            };
        }
        public async Task<UserNotificationRequest> FindById(Guid Id)
        {
            var userNotification = await _userNotificationUnitOfWork.Repository<VwUserNotification>().FindAsync(n => n.Id == Id);
            return _mapper.MapModel<VwUserNotification, UserNotificationRequest>(userNotification);
        }
        public async Task<bool> Save(UserNotificationRequest dto, Guid userId)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (userId == Guid.Empty) throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

            try
            {
                await _userNotificationUnitOfWork.BeginTransactionAsync();

                var notification = new TbNotification
                {
                    Id = dto.Id,
                    Message = dto.Message,
                    Title = dto.Title,
                };

                // Save or update notification using async API
                var saveResult = await _userNotificationUnitOfWork.TableRepository<TbNotification>().SaveAsync(notification, userId);
                if (!saveResult.Success)
                    throw new Exception(NotifiAndAlertsResources.SaveFailed);

                var notificationId = saveResult.Id;

                if (dto.Id != Guid.Empty)
                {
                    var existingUserNotifications = await _userNotificationUnitOfWork.TableRepository<TbUserNotification>()
                        .GetAsync(un => un.NotificationId == dto.Id && un.UserId == dto.UserId);

                    foreach (var userNotification in existingUserNotifications)
                    {
                        var deleted = await _userNotificationUnitOfWork.TableRepository<TbUserNotification>().HardDeleteAsync(userNotification.Id);
                        if (!deleted)
                            throw new Exception(NotifiAndAlertsResources.DeleteFailed);
                    }
                }

                // Get or create user notification
                var userNotificationEntity = new TbUserNotification()
                {
                    NotificationId = notificationId,
                    UserId = dto.UserId,
                    IsRead = dto.IsRead,
                };
                var createResult = await _userNotificationUnitOfWork.TableRepository<TbUserNotification>().CreateAsync(userNotificationEntity, userId);
                if (!createResult.Success)
                    throw new Exception(NotifiAndAlertsResources.SaveFailed);

                await _userNotificationUnitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                _userNotificationUnitOfWork.Rollback();
                throw;
            }
        }
        public async Task<UserNotificationResult<bool>> MarkAsRead(IEnumerable<UserNotificationRequest> userNotificationRequests, string userId)
        {
            if (userNotificationRequests == null || !userNotificationRequests.Any()) throw new ArgumentNullException(nameof(userNotificationRequests));
            if (Guid.Parse(userId) == Guid.Empty) throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

            try
            {
                await _userNotificationUnitOfWork.BeginTransactionAsync();

                // Update user notifications
                foreach (var userNotificationRequest in userNotificationRequests)
                {
                    var userNotification = await _userNotificationUnitOfWork.TableRepository<TbUserNotification>()
                          .FindAsync(un => un.NotificationId == userNotificationRequest.Id && un.UserId == userId);

                    if (userNotification == null) continue;

                    userNotification.IsRead = true;
                    var updateResult = await _userNotificationUnitOfWork.TableRepository<TbUserNotification>().UpdateAsync(userNotification, Guid.Parse(userId));
                    if (!updateResult.Success)
                        throw new Exception(NotifiAndAlertsResources.SaveFailed);
                }

                var userNotifications = await _userNotificationUnitOfWork.TableRepository<TbUserNotification>().GetAsync(n => n.UserId == userId);
                var unReadCount = userNotifications.Count(n => n.IsRead == false);
                var totalCount = userNotifications.Count();
                await _userNotificationUnitOfWork.CommitAsync();
                return new UserNotificationResult<bool>()
                {
                    Value = true,
                    UnReadCount = unReadCount,
                    TotalCount = totalCount
                };
            }
            catch
            {
                _userNotificationUnitOfWork.Rollback();
                throw;
            }
        }
        public async Task<bool> Delete(Guid id, Guid userId)
        {
            try
            {
                await _userNotificationUnitOfWork.BeginTransactionAsync();
                await _userNotificationUnitOfWork.TableRepository<TbNotification>().UpdateCurrentStateAsync(id, userId, true);
                var userNotifications = await _userNotificationUnitOfWork.TableRepository<TbUserNotification>().GetAsync(x => x.NotificationId == id);
                if (userNotifications?.Count() > 0)
                {
                    foreach (var userNotification in userNotifications)
                    {
                        var isUpdatedCurrentState = await _userNotificationUnitOfWork.TableRepository<TbUserNotification>().UpdateCurrentStateAsync(userNotification.Id, userId, true);
                        if (!isUpdatedCurrentState)
                            throw new Exception(NotifiAndAlertsResources.DeleteFailed);
                    }
                }
                await _userNotificationUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _userNotificationUnitOfWork.Rollback();
                throw new Exception(string.Format(ValidationResources.DeleteEntityError, GeneralResources.Notification), ex);
            }
        }
    }
}
