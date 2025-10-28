using BL.Contracts.GeneralService.Notification;
using BL.Contracts.IMapper;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using Domains.Entities.Notification;
using Resources;
using Serilog;
using Shared.GeneralModels.Parameters.Notification;
using Shared.GeneralModels.SearchCriteriaModels;
using Shared.ResultModels;

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
        public UserNotificationResult<PaginatedDataModel<UserNotificationRequest>> GetPage(BaseSearchCriteriaModel criteriaModel, string userId)
        {
            //if (criteriaModel == null)
            //    throw new ArgumentNullException(nameof(criteriaModel));

            //if (criteriaModel.PageNumber < 1)
            //    throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            //if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
            //    throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            //// Base filter for active entities
            //Expression<Func<VwUserNotification, bool>> filter = x => x.UserId == userId;

            //// Apply search term if provided
            //if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
            //{
            //    string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
            //    filter = x => (x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm)) ||
            //                 (x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm)) ||
            //                 (x.DescriptionAr != null && x.DescriptionAr.ToLower().Contains(searchTerm)) ||
            //                 (x.DescriptionEn != null && x.DescriptionEn.ToLower().Contains(searchTerm));
            //}

            //var entitiesList = _userNotificationUnitOfWork.Repository<VwUserNotification>().GetPage(
            //    criteriaModel.PageNumber,
            //    criteriaModel.PageSize,
            //    filter, orderBy: q => q.OrderByDescending(n => n.CreatedDateUtc));
            //var userNotifications = _userNotificationUnitOfWork.TableRepository<TbUserNotification>().Get(n => n.UserId == userId);
            //var unReadCount = userNotifications.Count(n => n.IsRead == false);
            //var totalCount = userNotifications.Count();
            //var dtoList = _mapper.MapList<VwUserNotification, UserNotificationRequest>(entitiesList.Items);

            //return new UserNotificationResult<PaginatedDataModel<UserNotificationRequest>>()
            //{
            //    Value = new PaginatedDataModel<UserNotificationRequest>(dtoList, entitiesList.TotalRecords),
            //    UnReadCount = unReadCount,
            //    TotalCount = totalCount
            //};

            throw new NotImplementedException();
        }
        public UserNotificationResult<IEnumerable<UserNotificationRequest>> GetAll(string userId)
        {
            //var allUserNotifications = _userNotificationUnitOfWork.Repository<VwUserNotification>().Get(n => n.UserId == userId);
            //var userNotifications = allUserNotifications.OrderByDescending(n => n.CreatedDateUtc).Take(10);
            //var unReadCount = allUserNotifications.Count(n => n.IsRead == false);
            //var totalCount = allUserNotifications.Count();
            //return new UserNotificationResult<IEnumerable<UserNotificationRequest>>()
            //{
            //    Value = _mapper.MapList<VwUserNotification, UserNotificationRequest>(userNotifications),
            //    UnReadCount = unReadCount,
            //    TotalCount = totalCount
            //};

            throw new NotImplementedException();
        }
        public UserNotificationRequest FindById(Guid Id)
        {
            //var userNotification = _userNotificationUnitOfWork.Repository<VwUserNotification>().Find(n => n.Id == Id);
            //return _mapper.MapModel<VwUserNotification, UserNotificationRequest>(userNotification);

            throw new NotImplementedException();
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
                    TitleAr = dto.TitleAr,
                    TitleEn = dto.TitleEn,
                    DescriptionAr = dto.DescriptionAr,
                    DescriptionEn = dto.DescriptionEn,
                };

                // Save or update notification
                var isSaved = _userNotificationUnitOfWork.TableRepository<TbNotification>().Save(notification, userId, out Guid notificationId);
                if (!isSaved)
                    throw new Exception(NotifiAndAlertsResources.SaveFailed);

                if (dto.Id != Guid.Empty)
                {
                    foreach (var userNotification in _userNotificationUnitOfWork.TableRepository<TbUserNotification>()
                        .Get(un => un.NotificationId == dto.Id && un.UserId == dto.UserId))
                    {
                        var isDeleted = _userNotificationUnitOfWork.TableRepository<TbUserNotification>().HardDelete(userNotification.Id);
                        if (!isDeleted)
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
                isSaved = _userNotificationUnitOfWork.TableRepository<TbUserNotification>().Create(userNotificationEntity, userId, out Guid userNotificationId);
                if (!isSaved)
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
            if (userNotificationRequests.Count() == 0) throw new ArgumentNullException(nameof(userNotificationRequests));
            if (Guid.Parse(userId) == Guid.Empty) throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

            try
            {
                await _userNotificationUnitOfWork.BeginTransactionAsync();

                // Update user notifications
                foreach (var userNotificationRequest in userNotificationRequests)
                {
                    var userNotification = _userNotificationUnitOfWork.TableRepository<TbUserNotification>()
                          .Find(un => un.NotificationId == userNotificationRequest.Id && un.UserId == userId);
                    userNotification.IsRead = true;
                    var isSaved = _userNotificationUnitOfWork.TableRepository<TbUserNotification>().Update(userNotification, Guid.Parse(userId), out Guid userNotificationId);
                    if (!isSaved)
                        throw new Exception(NotifiAndAlertsResources.SaveFailed);
                }
                var userNotifications = _userNotificationUnitOfWork.TableRepository<TbUserNotification>().Get(n => n.UserId == userId);
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
                _userNotificationUnitOfWork.TableRepository<TbNotification>().UpdateCurrentState(id, userId, 0);
                var userNotifications = _userNotificationUnitOfWork.TableRepository<TbUserNotification>().Get(x => x.NotificationId == id);
                if (userNotifications?.Count() > 0)
                {
                    foreach (var userNotification in userNotifications)
                    {
                        var isUpdatedCurrentState = _userNotificationUnitOfWork.TableRepository<TbUserNotification>().UpdateCurrentState(userNotification.Id, userId, 0);
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
