using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Service.WithdrawalMethod;
using BL.Services.Base;
using Common.Enumerations.FieldType;
using DAL.Contracts.UnitOfWork;
using DAL.Exceptions;
using DAL.ResultModels;
using Domains.Entities.WithdrawalMethods;
using Domains.Views.WithdrawalMethods;
using Resources;
using Serilog;
using Shared.DTOs.WithdrawalMethod;
using Shared.DTOs.WithdrawelMethod;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace BL.Services.WithdrawalMethod
{
    public class WithdrawalMethodService : BaseService<TbWithdrawalMethod, WithdrawalMethodDto>, IWithdrawalMethodService
    {

        private readonly IBaseMapper _mapper;
        private readonly IFileUploadService _fileUploadService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public WithdrawalMethodService(
            IUnitOfWork unitOfWork,
            IBaseMapper mapper,
            IFileUploadService fileUploadService,
            IImageProcessingService imageProcessingService,
            ILogger logger) : base(unitOfWork.TableRepository<TbWithdrawalMethod>(), mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileUploadService = fileUploadService;
            _imageProcessingService = imageProcessingService;
            _logger = logger;
        }

        public override async Task<IEnumerable<WithdrawalMethodDto>> GetAllAsync()
        {
            try
            {
                // Get all active Withdrawal methods
                var WithdrawalMethods = await _unitOfWork.Repository<VwWithdrawalMethodsWithFields>().GetAllAsync();

                // Map to DTOs
                var WithdrawalMethodsDto = _mapper.MapList<VwWithdrawalMethodsWithFields, WithdrawalMethodDto>(WithdrawalMethods);
                return WithdrawalMethodsDto;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ValidationResources.LoadDataFailed);
                throw new NotFoundException(
                    ValidationResources.LoadDataFailed,
                    _logger);
            }
        }

        public override async Task<WithdrawalMethodDto> FindByIdAsync(Guid Id)
        {
            Expression<Func<VwWithdrawalMethodsWithFields, bool>> filter = x => x.Id == Id;
            var vwWithdrawalMethodsWithFields = await _unitOfWork.Repository<VwWithdrawalMethodsWithFields>().FindAsync(filter);

            if (vwWithdrawalMethodsWithFields == null)
                throw new Exception(string.Format(ValidationResources.EntityNotFound, ECommerceResources.Attribute));

            return _mapper.MapModel<VwWithdrawalMethodsWithFields, WithdrawalMethodDto>(vwWithdrawalMethodsWithFields);
        }

        public override async Task<SaveResult> SaveAsync(WithdrawalMethodDto dto, Guid userId)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (userId == Guid.Empty) throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Process image if provided
                if (_fileUploadService.ValidateFile(dto.ImageFile).isValid)
                    dto.ImagePath = await ProcessImage(dto.ImageFile);

                // Map DTO to entity
                var entity = _mapper.MapModel<WithdrawalMethodDto, TbWithdrawalMethod>(dto);

                Guid WithdrawalMethodId = dto.Id;
                Guid newWithdrawalMethodId;
                bool isUpdate = WithdrawalMethodId != Guid.Empty;

                // Save or update Withdrawal method
                if (isUpdate)
                {
                    WithdrawalMethodId = (await _unitOfWork.TableRepository<TbWithdrawalMethod>()
                        .UpdateAsync(entity, userId)).Id;

                    // Get Withdrawal method association
                    var userWithdrawalMethods = await GetUserWithdrawalMethodIds(WithdrawalMethodId, userId);

                    foreach (var userWithdrawalMethod in userWithdrawalMethods)
                    {
                        // Update existing user Withdrawal method
                        // Handle fields
                        await HandleWithdrawalMethodFieldsAsync(dto.Fields, WithdrawalMethodId, userId, userWithdrawalMethod.Id, isUpdate);
                    }
                }
                else
                {
                    var save = await _unitOfWork.TableRepository<TbWithdrawalMethod>()
                        .SaveAsync(entity, userId);

                    newWithdrawalMethodId = save.Id;

                    if (!save.Success)
                    {
                        throw new Exception(NotifiAndAlertsResources.SaveFailed);
                    }
                }

                await _unitOfWork.CommitAsync();
                return new SaveResult() { Success = true };
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public override async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.TableRepository<TbWithdrawalMethod>().UpdateCurrentStateAsync(id, userId, true);
                var fields = await _unitOfWork.TableRepository<TbField>().GetAsync(x => x.WithdrawalMethodId == id);
                if (fields?.Count() > 0)
                {
                    foreach (var field in fields)
                    {
                        await _unitOfWork.TableRepository<TbField>().UpdateCurrentStateAsync(field.Id, userId, true);
                    }
                }
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(string.Format(ValidationResources.DeleteEntityError, GeneralResources.WithdrawalMethod), ex);
            }
        }

        #region Helper Methods

        private async Task<List<TbUserWithdrawalMethod>> GetUserWithdrawalMethodIds(Guid WithdrawalMethodId, Guid userId)
        {
            // Look for existing user-Withdrawal method bridge
            var userWithdrawalMethod = await _unitOfWork.TableRepository<TbUserWithdrawalMethod>()
                .GetAsync(upm => upm.WithdrawalMethodId == WithdrawalMethodId);

            return userWithdrawalMethod.ToList();
        }

        private async Task HandleWithdrawalMethodFieldsAsync(IEnumerable<FieldDto> fieldDtos, Guid WithdrawalMethodId,
                                               Guid userId, Guid userWithdrawalMethodId, bool isUpdate)
        {
            // Get existing fields if we're updating
            var existingFields = isUpdate
                ? await _unitOfWork.TableRepository<TbField>().GetAsync(f => f.WithdrawalMethodId == WithdrawalMethodId)
                : new List<TbField>();

            // Get existing bridges
            var existingBridges = isUpdate
                ? await _unitOfWork.TableRepository<TbWithdrawalMethodField>().GetAsync(b => b.UserWithdrawalMethodId == userWithdrawalMethodId)
                : new List<TbWithdrawalMethodField>();

            bool hasNewFields = fieldDtos?.Any() == true;

            if (isUpdate && hasNewFields)
            {
                var fieldDtosList = fieldDtos.ToList();

                // Find fields to delete (exist in DB but not in incoming data)
                var fieldsToDelete = existingFields.Where(existingField =>
                    !fieldDtosList.Any(dto => AreFieldsEqual(dto, existingField))).ToList();

                // Find fields to add (exist in incoming data but not in DB)
                var fieldsToAdd = fieldDtosList.Where(dto =>
                    !existingFields.Any(existingField => AreFieldsEqual(dto, existingField))).ToList();

                // Find fields to update (exist in both but with different values)
                var fieldsToUpdate = new List<(FieldDto dto, TbField existing)>();

                foreach (var dto in fieldDtosList)
                {
                    var existingField = existingFields.FirstOrDefault(f => f.Id == dto.Id);
                    if (existingField != null && !AreFieldsEqual(dto, existingField))
                    {
                        fieldsToUpdate.Add((dto, existingField));
                    }
                }

                // Delete removed fields and their bridges
                if (fieldsToDelete.Any())
                {
                    SoftDeleteEntities(fieldsToDelete, userId);

                    var bridgesToDelete = existingBridges.Where(bridge =>
                        fieldsToDelete.Any(field => field.Id == bridge.FieldId)).ToList();
                    SoftDeleteEntities(bridgesToDelete, userId);
                }

                // Add new fields
                foreach (var fieldDto in fieldsToAdd)
                {
                    var fieldEntity = _mapper.MapModel<FieldDto, TbField>(fieldDto);
                    fieldEntity.WithdrawalMethodId = WithdrawalMethodId;

                    var fieldId = (await _unitOfWork.TableRepository<TbField>()
                        .CreateAsync(fieldEntity, userId)).Id;

                    await _unitOfWork.TableRepository<TbWithdrawalMethodField>()
                        .CreateAsync(new TbWithdrawalMethodField()
                        {
                            FieldId = fieldId,
                            UserWithdrawalMethodId = userWithdrawalMethodId
                        }, userId);
                }

                // Update existing fields
                foreach (var (dto, existingField) in fieldsToUpdate)
                {
                    // Update field properties manually or using mapper
                    existingField.TitleAr = dto.TitleAr;
                    existingField.TitleEn = dto.TitleEn;
                    existingField.FieldType = dto.FieldType;
                    existingField.WithdrawalMethodId = WithdrawalMethodId;

                    await _unitOfWork.TableRepository<TbField>()
                        .UpdateAsync(existingField, userId);
                }
            }
            else if (!isUpdate && hasNewFields)
            {
                // Handle creation of new Withdrawal method with fields
                var fieldEntities = fieldDtos.Select(fieldDto =>
                {
                    var fieldEntity = _mapper.MapModel<FieldDto, TbField>(fieldDto);
                    fieldEntity.WithdrawalMethodId = WithdrawalMethodId;
                    return fieldEntity;
                }).ToList();

                foreach (var field in fieldEntities)
                {
                    var fieldId = (await _unitOfWork.TableRepository<TbField>()
                        .CreateAsync(field, userId)).Id;

                    await _unitOfWork.TableRepository<TbWithdrawalMethodField>()
                        .CreateAsync(new TbWithdrawalMethodField()
                        {
                            FieldId = fieldId,
                            UserWithdrawalMethodId = userWithdrawalMethodId
                        }, userId);
                }
            }
        }

        // Helper method to compare fields for equality
        private bool AreFieldsEqual(FieldDto dto, TbField entity)
        {
            // Compare based on TbField properties
            return dto.Id == entity.Id &&
                   dto.TitleAr == entity.TitleAr &&
                   dto.TitleEn == entity.TitleEn &&
                   dto.FieldType == (FieldType)entity.FieldType;
            // Add more FieldDto property comparisons as needed
        }

        // Generic method for soft deleting entities
        private async Task SoftDeleteBridgesForWithdrawalMethodAsync(Guid userWithdrawalMethodId, Guid userId)
        {
            // Soft delete all Withdrawal method field bridges
            var oldBridges = await _unitOfWork.TableRepository<TbWithdrawalMethodField>()
                .GetAsync(f => f.UserWithdrawalMethodId == userWithdrawalMethodId);
            SoftDeleteEntities(oldBridges, userId);
        }

        private void SoftDeleteEntities<T>(IEnumerable<T> entities, Guid userId) where T : BaseEntity
        {
            foreach (var entity in entities)
            {
                _unitOfWork.TableRepository<T>().UpdateCurrentStateAsync(entity.Id, userId, true);
            }
        }

        private async Task<string> ProcessImage(string image)
        {
            // Validate image
            if (string.IsNullOrEmpty(image))
            {
                throw new ValidationException(ValidationResources.ImageRequired);
            }

            var imageValidation = _fileUploadService.ValidateFile(image);
            if (!imageValidation.isValid)
            {
                throw new ValidationException(imageValidation.errorMessage);
            }

            try
            {
                // Process and optimize image
                var imageBytes = await _fileUploadService.GetFileBytesAsync(image);
                var resizedImage = _imageProcessingService.ResizeImagePreserveAspectRatio(imageBytes, 800, 600);
                var webpImage = _imageProcessingService.ConvertToWebP(resizedImage);

                // Upload and return path
                return await _fileUploadService.UploadFileAsync(webpImage, Path.Combine("Images", "WithdrawalMethods"));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ValidationResources.ErrorProcessingImage);
                throw new ApplicationException(ValidationResources.ErrorProcessingImage, ex);
            }
        }

        #endregion
    }
}
