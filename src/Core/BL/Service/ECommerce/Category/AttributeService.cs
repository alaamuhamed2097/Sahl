using BL.Contracts.IMapper;
using BL.Contracts.Service.ECommerce.Category;
using BL.Service.Base;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using DAL.ResultModels;
using Domains.Entities.Category;
using Domains.Entities.Item;
using Domains.Views.Category;
using Resources;
using Shared.DTOs.ECommerce.Category;
using Shared.DTOs.ECommerce.Item;
using Shared.GeneralModels.SearchCriteriaModels;
using Shared.ResultModels;
using System.Linq.Expressions;

namespace BL.Service.ECommerce.Category
{
    public class AttributeService : BaseService<TbAttribute, AttributeDto>, IAttributeService
    {
        private readonly IUnitOfWork _attributeUnitOfWork;
        private readonly IBaseMapper _mapper;

        public AttributeService(IBaseMapper mapper, IUnitOfWork attributeUnitOfWork)
            : base(attributeUnitOfWork.TableRepository<TbAttribute>(), mapper)
        {
            _mapper = mapper;
            _attributeUnitOfWork = attributeUnitOfWork;
        }

        public async Task<PaginatedDataModel<AttributeDto>> GetPage(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter for active entities
            Expression<Func<TbAttribute, bool>> filter = x => x.CurrentState == 1;

            // Apply search term if provided
            if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
            {
                string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
                filter = x => x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm) ||
                             x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm);
            }

            // Create ordering function based on SortBy and SortDirection
            Func<IQueryable<TbAttribute>, IOrderedQueryable<TbAttribute>> orderBy = null;

            if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
            {
                var sortBy = criteriaModel.SortBy.ToLower();
                var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

                orderBy = query =>
                {
                    return sortBy switch
                    {
                        "titlear" => isDescending ? query.OrderByDescending(x => x.TitleAr) : query.OrderBy(x => x.TitleAr),
                        "titleen" => isDescending ? query.OrderByDescending(x => x.TitleEn) : query.OrderBy(x => x.TitleEn),
                        "title" => isDescending ? query.OrderByDescending(x => x.TitleAr) : query.OrderBy(x => x.TitleAr),
                        "fieldtype" => isDescending ? query.OrderByDescending(x => x.FieldType) : query.OrderBy(x => x.FieldType),
                        "createddateutc" => isDescending ? query.OrderByDescending(x => x.CreatedDateUtc) : query.OrderBy(x => x.CreatedDateUtc),
                        _ => query.OrderByDescending(a => a.CreatedDateUtc) // Default sorting
                    };
                };
            }
            else
            {
                // Default ordering
                orderBy = query => query.OrderByDescending(a => a.CreatedDateUtc);
            }

            var entitiesList = await _attributeUnitOfWork.Repository<TbAttribute>().GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy: orderBy);

            var attributes = _mapper.MapList<TbAttribute, AttributeDto>(entitiesList.Items);

            return new PaginatedDataModel<AttributeDto>(attributes, entitiesList.TotalRecords);
        }

        public override async Task<AttributeDto> FindByIdAsync(Guid Id)
        {
            Expression<Func<VwAttributeWithOptions, bool>> filter = x => x.Id == Id;
            var vwAttributeWithOptions = await _attributeUnitOfWork.Repository<VwAttributeWithOptions>().FindAsync(filter);

            if (vwAttributeWithOptions == null)
                throw new Exception(string.Format(ValidationResources.EntityNotFound, ECommerceResources.Attribute));

            return _mapper.MapModel<VwAttributeWithOptions, AttributeDto>(vwAttributeWithOptions);
        }

        public override async Task<SaveResult> SaveAsync(AttributeDto dto, Guid userId)
        {
            try
            {
                // Validate input
                if (dto == null) throw new ArgumentNullException(nameof(dto));
                if (userId == Guid.Empty) throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

                // Map and create main attribute entity
                var entity = _mapper.MapModel<AttributeDto, TbAttribute>(dto);

                // Begin transaction
                await _attributeUnitOfWork.BeginTransactionAsync();

                // delete old attribute options if any
                if (dto.Id != Guid.Empty && dto.AttributeOptions != null && dto.AttributeOptions.Any())
                {
                    var attributeOptions = await _attributeUnitOfWork.TableRepository<TbAttributeOption>().GetAsync(c => c.AttributeId == dto.Id);
                    foreach (var attributeOption in attributeOptions)
                    {
                        await _attributeUnitOfWork.TableRepository<TbAttributeOption>().HardDeleteAsync(attributeOption.Id);
                    }
                }

                // Save main attribute
                var saveResult = await _attributeUnitOfWork.TableRepository<TbAttribute>().SaveAsync(entity, userId);

                // Prepare attribute options with display order management
                List<AttributeOptionDto> optionsToCreate;
                if (dto.AttributeOptions != null && dto.AttributeOptions.Any())
                {
                    // Sort by current DisplayOrder to maintain intended order
                    var sortedOptions = dto.AttributeOptions.OrderBy(o => o.DisplayOrder).ToList();

                    // Reassign display orders to be consecutive starting at 1
                    for (int i = 0; i < sortedOptions.Count; i++)
                    {
                        sortedOptions[i].DisplayOrder = i + 1;
                    }
                    optionsToCreate = sortedOptions;
                }
                else
                {
                    optionsToCreate = new List<AttributeOptionDto>();
                }

                // Save attribute options with managed display order
                foreach (var attributeOption in optionsToCreate)
                {
                    var Option = _mapper.MapModel<AttributeOptionDto, TbAttributeOption>(attributeOption);
                    Option.AttributeId = saveResult.Id;
                    var createResult = await _attributeUnitOfWork.TableRepository<TbAttributeOption>().CreateAsync(Option, userId);
                }

                // Commit transaction
                await _attributeUnitOfWork.CommitAsync();
                return new() { Success = true, Id = saveResult.Id };
            }
            catch (Exception ex)
            {
                // Rollback on error
                _attributeUnitOfWork.Rollback();
                throw new Exception(string.Format(ValidationResources.SaveEntityError, ECommerceResources.Attribute), ex);
            }
        }

        public async Task<DeleteResult> DeleteAsync(Guid id, string userId)
        {
            var errors = new List<string>();
            if (id == Guid.Empty)
            {
                throw new ArgumentException(NotifiAndAlertsResources.InvalidInput, nameof(id));
            }
            try
            {
                await _attributeUnitOfWork.BeginTransactionAsync();

                // Prevent deletion if attribute is in use by items
                var categoryAttributeEntities = await _attributeUnitOfWork.TableRepository<TbCategoryAttribute>().GetAsync(i => i.AttributeId == id && i.CurrentState == 1);
                var categoriesIds = categoryAttributeEntities.Select(ci => ci.CategoryId).ToList();
                var categoryEntities = await _attributeUnitOfWork.TableRepository<TbCategory>().GetAsync(i => categoriesIds.Contains(i.Id) && i.CurrentState == 1);
                var categories = _mapper.MapList<TbCategory, CategoryDto>(categoryEntities);

                var itemAttributeEntities = await _attributeUnitOfWork.TableRepository<TbItemAttribute>().GetAsync(i => i.AttributeId == id && i.CurrentState == 1);
                var itemsIds = itemAttributeEntities.Select(ci => ci.ItemId).ToList();
                var itemEntities = await _attributeUnitOfWork.TableRepository<TbItem>().GetAsync(i => itemsIds.Contains(i.Id) && i.CurrentState == 1);
                var items = _mapper.MapList<TbItem, ItemDto>(itemEntities);

                if (categories != null && categories.Any())
                {
                    var categoryTitles = categories.Select(i => i.Title).Take(5).ToList(); // Take up to 5 to avoid long messages
                    var formattedTitles = string.Join(", ", categoryTitles);
                    string errorMessage;
                    if (categoryTitles.Count >= 5)
                    {
                        errorMessage = string.Format(
                            NotifiAndAlertsResources.EntityCannotBeDeletedInUse,
                            ECommerceResources.Attribute,
                            ECommerceResources.Categories,
                            formattedTitles
                            ) + "...";
                    }
                    else
                    {
                        errorMessage = string.Format(
                            NotifiAndAlertsResources.EntityCannotBeDeletedInUse,
                            ECommerceResources.Attribute,
                            ECommerceResources.Categories,
                            formattedTitles);
                    }
                    errors.Add(errorMessage);
                }

                if (items != null && items.Any())
                {
                    var itemsTitles = items.Select(i => i.Title).Take(5).ToList(); // Take up to 5 to avoid long messages
                    var formattedTitles = string.Join(", ", itemsTitles);
                    string errorMessage;
                    if (itemsTitles.Count >= 5)
                    {
                        errorMessage = string.Format(
                            NotifiAndAlertsResources.EntityCannotBeDeletedInUse,
                            ECommerceResources.Attribute,
                            ECommerceResources.Adverts,
                            formattedTitles
                            ) + "...";
                    }
                    else
                    {
                        errorMessage = string.Format(
                            NotifiAndAlertsResources.EntityCannotBeDeletedInUse,
                            ECommerceResources.Attribute,
                            ECommerceResources.Adverts,
                            formattedTitles);
                    }
                    errors.Add(errorMessage);
                }

                if (errors.Any())
                    return new DeleteResult() { Success = false, Errors = errors };

                // Get attribute options before deletion to manage display order
                var attributeOptions = await _attributeUnitOfWork.TableRepository<TbAttributeOption>().GetAsync(x => x.AttributeId == id && x.CurrentState == 1);

                // Update attribute state
                await _attributeUnitOfWork.TableRepository<TbAttribute>().UpdateCurrentStateAsync(id, new Guid(userId));

                // Update attribute options state and manage display order
                if (attributeOptions?.Count() > 0)
                {
                    // Sort by display order to process in sequence
                    var sortedOptions = attributeOptions.OrderBy(o => o.DisplayOrder).ToList();

                    foreach (var option in sortedOptions)
                    {
                        await _attributeUnitOfWork.TableRepository<TbAttributeOption>().UpdateCurrentStateAsync(option.Id, new Guid(userId));
                    }
                }

                await _attributeUnitOfWork.CommitAsync();
                return new DeleteResult() { Success = true, Errors = errors };
            }
            catch (Exception ex)
            {
                _attributeUnitOfWork.Rollback();
                throw new Exception(string.Format(ValidationResources.DeleteEntityError, ECommerceResources.Attribute), ex);
            }
        }

        // New method to update display orders in bulk
        public async Task<bool> UpdateAttributeOptionsDisplayOrderAsync(Dictionary<Guid, int> optionDisplayOrders, Guid userId)
        {
            try
            {
                if (optionDisplayOrders == null || !optionDisplayOrders.Any())
                    return true; // Nothing to update

                // Prepare the dictionary for bulk update
                var entityFieldValues = new Dictionary<Guid, Dictionary<string, object>>();
                foreach (var kvp in optionDisplayOrders)
                {
                    entityFieldValues[kvp.Key] = new Dictionary<string, object>
            {
                { "DisplayOrder", kvp.Value }
            };
                }

                // Use the bulk update method
                var (success, _) = await _attributeUnitOfWork.TableRepository<TbAttributeOption>().UpdateBulkFieldsAsync(entityFieldValues, userId);
                return success;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating attribute options display order", ex);
            }
        }

        // Helper method to shift display orders when inserting a new option
        private async Task ShiftAttributeOptionDisplayOrdersForInsertAsync(List<TbAttributeOption> optionsToShift, Guid userId)
        {
            if (optionsToShift == null || !optionsToShift.Any())
                return;

            // Prepare dictionary for bulk update
            var entityFieldValues = new Dictionary<Guid, Dictionary<string, object>>();

            // Update in descending order to avoid constraint violations
            foreach (var option in optionsToShift.OrderByDescending(o => o.DisplayOrder))
            {
                entityFieldValues[option.Id] = new Dictionary<string, object>
        {
            { "DisplayOrder", option.DisplayOrder + 1 }
        };
            }

            // Use bulk update
            await _attributeUnitOfWork.TableRepository<TbAttributeOption>().UpdateBulkFieldsAsync(entityFieldValues, userId);
        }

        // Helper method to shift display orders when deleting an option
        private async Task ShiftAttributeOptionDisplayOrdersAfterDeleteAsync(List<TbAttributeOption> optionsToShift, Guid userId)
        {
            if (optionsToShift == null || !optionsToShift.Any())
                return;

            // Prepare dictionary for bulk update
            var entityFieldValues = new Dictionary<Guid, Dictionary<string, object>>();

            // Update in ascending order to avoid constraint violations
            foreach (var option in optionsToShift.OrderBy(o => o.DisplayOrder))
            {
                entityFieldValues[option.Id] = new Dictionary<string, object>
        {
            { "DisplayOrder", option.DisplayOrder - 1 }
        };
            }

            // Use bulk update
            await _attributeUnitOfWork.TableRepository<TbAttributeOption>().UpdateBulkFieldsAsync(entityFieldValues, userId);
        }

        /// <summary>
        /// Get all attributes for a specific category with their options
        /// </summary>
        public async Task<IEnumerable<CategoryAttributeDto>> GetByCategoryIdAsync(Guid categoryId)
        {
            try
            {
                if (categoryId == Guid.Empty)
                    throw new ArgumentException("Category ID cannot be empty", nameof(categoryId));

                // Get category attributes for the specified category
                var categoryAttributes = await _attributeUnitOfWork.TableRepository<TbCategoryAttribute>()
                    .GetAsync(ca => ca.CategoryId == categoryId && ca.CurrentState == 1);

                if (!categoryAttributes.Any())
                    return new List<CategoryAttributeDto>();

                // Get the attribute IDs
                var attributeIds = categoryAttributes.Select(ca => ca.AttributeId).ToList();

                // Get attributes with options using the view
                var attributesWithOptions = await _attributeUnitOfWork.Repository<VwAttributeWithOptions>()
                    .GetAsync(a => attributeIds.Contains(a.Id));

                // Map to DTOs
                var attributeDtos = _mapper.MapList<VwAttributeWithOptions, AttributeDto>(attributesWithOptions);

                // Build CategoryAttributeDto with combined information
                var result = new List<CategoryAttributeDto>();
                foreach (var categoryAttr in categoryAttributes.OrderBy(ca => ca.DisplayOrder))
                {
                    var attributeDto = attributeDtos.FirstOrDefault(a => a.Id == categoryAttr.AttributeId);
                    if (attributeDto != null)
                    {
                        result.Add(new CategoryAttributeDto
                        {
                            Id = categoryAttr.Id,
                            CategoryId = categoryAttr.CategoryId,
                            AttributeId = categoryAttr.AttributeId,
                            TitleAr = attributeDto.TitleAr,
                            TitleEn = attributeDto.TitleEn,
                            FieldType = attributeDto.FieldType,
                            IsRangeFieldType = attributeDto.IsRangeFieldType,
                            MaxLength = attributeDto.MaxLength,
                            AffectsPricing = categoryAttr.AffectsPricing,
                            IsRequired = categoryAttr.IsRequired,
                            DisplayOrder = categoryAttr.DisplayOrder,
                            // FIX: Don't serialize - pass the list directly
                            AttributeOptionsJson = attributeDto.AttributeOptions ?? new List<AttributeOptionDto>()
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting attributes for category {categoryId}: {ex.Message}", ex);
            }
        }
    }
}
