using BL.Contracts.IMapper;
using BL.Contracts.Service.WithdrawalMethod;
using BL.Services.Base;
using DAL.Contracts.UnitOfWork;
using DAL.Exceptions;
using DAL.ResultModels;
using Domains.Entities.WithdrawalMethods;
using Domains.Views.WithdrawalMethods;
using Resources;
using Serilog;
using Shared.DTOs.WithdrawalMethod;
using System.Linq.Expressions;

namespace BL.Services.WithdrawalMethod
{
    public class UserWithdrawalMethodService : BaseService<TbUserWithdrawalMethod, UserWithdrawalMethodDto>, IUserWithdrawalMethodService
    {
        private readonly IUnitOfWork _userWithdrawalMethodRepository;
        private readonly IBaseMapper _mapper;
        private readonly ILogger _logger;
        public UserWithdrawalMethodService(IBaseMapper mapper, IUnitOfWork fieldRepository, ILogger logger) : base(fieldRepository.TableRepository<TbUserWithdrawalMethod>(), mapper)
        {
            _mapper = mapper;
            _userWithdrawalMethodRepository = fieldRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<WithdrawalMethodsFieldsValuesDto>> GetAllWithdrawalFieldsValues(string UserId)
        {
            try
            {
                // Get all active Withdrawal methods with their fields
                var WithdrawalMethodsFieldsValues = await _userWithdrawalMethodRepository.Repository<VwWithdrawalMethodsFieldsValues>().GetAllAsync();
                // Map to DTOs with fields

                var result = _mapper.MapList<VwWithdrawalMethodsFieldsValues, WithdrawalMethodsFieldsValuesDto>(WithdrawalMethodsFieldsValues);
                var filteredWithdrawalMethods = result.Select(pm => new WithdrawalMethodsFieldsValuesDto
                {
                    WithdrawalMethodId = pm.WithdrawalMethodId,
                    TitleAr = pm.TitleAr,
                    TitleEn = pm.TitleEn,
                    ImagePath = pm.ImagePath,
                    FieldsJson = pm.FieldsJson?.Where(field => field.UserId == UserId).ToList()
                }).ToList();

                return filteredWithdrawalMethods;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ValidationResources.LoadDataFailed);
                throw new NotFoundException(
                    ValidationResources.LoadDataFailed,
                    _logger);
            }
        }
        public async Task<WithdrawalMethodsFieldsValuesDto> FindWithdrawalFieldsValuesById(Guid WithdrawalMethodId, string UserId)
        {
            // Validate input
            if (WithdrawalMethodId == Guid.Empty)
            {
                throw new ArgumentException(NotifiAndAlertsResources.InvalidInput, nameof(WithdrawalMethodId));
            }

            try
            {
                // Find the main Withdrawal method
                Expression<Func<VwWithdrawalMethodsFieldsValues, bool>> filter = x => x.WithdrawalMethodId == WithdrawalMethodId;
                var userWithdrawalMethod = await _userWithdrawalMethodRepository.Repository<VwWithdrawalMethodsFieldsValues>().FindAsync(filter);
                if (userWithdrawalMethod == null)
                {
                    throw new NotFoundException(
                        string.Format(ValidationResources.EntityNotFound, GeneralResources.WithdrawalMethod), _logger);
                }

                var result = _mapper.MapModel<VwWithdrawalMethodsFieldsValues, WithdrawalMethodsFieldsValuesDto>(userWithdrawalMethod);
                var filteredWithdrawalMethod = new WithdrawalMethodsFieldsValuesDto
                {
                    WithdrawalMethodId = result.WithdrawalMethodId,
                    TitleAr = result.TitleAr,
                    TitleEn = result.TitleEn,
                    ImagePath = result.ImagePath,
                    FieldsJson = result.FieldsJson?.Where(field => field.UserId == UserId).ToList()
                };

                return filteredWithdrawalMethod;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, string.Format(ValidationResources.EntityNotFound, GeneralResources.WithdrawalMethod), WithdrawalMethodId);
                throw new NotFoundException(
                    string.Format(ValidationResources.EntityNotFound, GeneralResources.WithdrawalMethod),
                    _logger);
            }
        }
        public override async Task<SaveResult> SaveAsync(UserWithdrawalMethodDto dto, Guid userId)
        {
            try
            {
                if (dto == null) throw new ArgumentNullException(nameof(dto));
                if (userId == Guid.Empty) throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

                // Map user Withdrawal method 
                var entity = _mapper.MapModel<UserWithdrawalMethodDto, TbUserWithdrawalMethod>(dto);

                // Begin transaction
                await _userWithdrawalMethodRepository.BeginTransactionAsync();

                var fieldEntities = new List<TbWithdrawalMethodField>();

                // Handle update case - delete old fields
                if (dto.WithdrawalMethodFields?.Count > 0 && dto.Id != Guid.Empty)
                {
                    foreach (var field in dto.WithdrawalMethodFields)
                    {
                        await _userWithdrawalMethodRepository.TableRepository<TbWithdrawalMethodField>()
                             .HardDeleteAsync(field.Id);
                    }

                    fieldEntities = _mapper.MapList<WithdrawalMethodFieldDto, TbWithdrawalMethodField>(dto.WithdrawalMethodFields).ToList();
                    fieldEntities.ForEach(f => f.Id = Guid.Empty);
                }
                else
                {
                    var tempEntity = new TbUserWithdrawalMethod
                    {
                        WithdrawalMethodId = entity.WithdrawalMethodId,
                        UserId = entity.UserId,
                    };

                    // Save Withdrawal method
                    var saveUserWithdrawalMethod = await _userWithdrawalMethodRepository.TableRepository<TbUserWithdrawalMethod>()
                        .SaveAsync(tempEntity, userId);

                    if (!saveUserWithdrawalMethod.Success)
                        throw new Exception(NotifiAndAlertsResources.SaveFailed);

                    // Prepare new fields
                    fieldEntities = (dto.WithdrawalMethodFields ?? new List<WithdrawalMethodFieldDto>())
                       .Select(fieldDto =>
                       {
                           var fieldEntity = _mapper.MapModel<WithdrawalMethodFieldDto, TbWithdrawalMethodField>(fieldDto);
                           fieldEntity.UserWithdrawalMethodId = saveUserWithdrawalMethod.Id;
                           return fieldEntity;
                       }).ToList();
                }

                // Add new fields
                if (fieldEntities.Any())
                {
                    var added = await _userWithdrawalMethodRepository.TableRepository<TbWithdrawalMethodField>()
                           .AddRangeAsync(fieldEntities, userId);
                }

                // Commit transaction
                await _userWithdrawalMethodRepository.CommitAsync();
                return new SaveResult() { Success = true };
            }
            catch (Exception ex)
            {
                await _userWithdrawalMethodRepository.RollbackAsync();
                _logger.Error(ex, string.Format(ValidationResources.SaveEntityError, GeneralResources.WithdrawalMethod), dto.Id);
                throw new Exception(ValidationResources.Error, ex);
            }
        }
        public override async Task<bool> DeleteAsync(Guid WithdrawalMethodId, Guid userId)
        {
            // Validate input
            if (WithdrawalMethodId == Guid.Empty)
            {
                throw new ArgumentException(NotifiAndAlertsResources.InvalidInput, nameof(WithdrawalMethodId));
            }

            if (userId == Guid.Empty)
            {
                throw new ArgumentException(UserResources.UserNotFound, nameof(userId));
            }

            try
            {
                // Start transaction
                await _userWithdrawalMethodRepository.BeginTransactionAsync();

                // Get the specific user Withdrawal method (not the Withdrawal method template)
                var userWithdrawalMethod = await _userWithdrawalMethodRepository.TableRepository<TbUserWithdrawalMethod>()
                    .FindAsync(uf => uf.WithdrawalMethodId == WithdrawalMethodId && uf.UserId == userId.ToString());

                if (userWithdrawalMethod == null)
                {
                    throw new KeyNotFoundException(string.Format(ValidationResources.EntityNotFound, GeneralResources.WithdrawalMethod));
                }

                // Delete all related Withdrawal method fields for this user Withdrawal method
                var WithdrawalMethodFields = await _userWithdrawalMethodRepository.TableRepository<TbWithdrawalMethodField>()
                    .GetAsync(x => x.UserWithdrawalMethodId == userWithdrawalMethod.Id);

                foreach (var methodField in WithdrawalMethodFields)
                {
                    await _userWithdrawalMethodRepository.TableRepository<TbWithdrawalMethodField>()
                         .UpdateCurrentStateAsync(methodField.Id, userId, false);
                }

                // Delete the user Withdrawal method
                await _userWithdrawalMethodRepository.TableRepository<TbUserWithdrawalMethod>()
                     .UpdateCurrentStateAsync(userWithdrawalMethod.Id, userId, false);

                // Commit transaction
                await _userWithdrawalMethodRepository.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Rollback on error
                await _userWithdrawalMethodRepository.RollbackAsync();

                // Log the error
                _logger.Error(ex, string.Format(ValidationResources.DeleteEntityError, GeneralResources.WithdrawalMethod), WithdrawalMethodId);

                throw new Exception(
                    string.Format(ValidationResources.DeleteEntityError, GeneralResources.WithdrawalMethod),
                    ex);
            }
        }
    }
}