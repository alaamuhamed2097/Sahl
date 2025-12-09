using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Service.ECommerce.Category;
using BL.Service.Base;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using Domains.Entities.Catalog.Category;
using Domains.Entities.Catalog.Item;
using Domains.Views.Category;
using Resources;
using Shared.DTOs.ECommerce.Category;
using Shared.DTOs.ECommerce.Item;
using Shared.GeneralModels.SearchCriteriaModels;
using Shared.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace BL.Service.ECommerce.Category
{
    public class CategoryService : BaseService<TbCategory, CategoryDto>, ICategoryService
    {
        private readonly IUnitOfWork _categoryUnitOfWork;
        private readonly IFileUploadService _fileUploadService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IBaseMapper _mapper;

        public CategoryService(IBaseMapper mapper,
            IUnitOfWork categoryUnitOfWork,
            IFileUploadService fileUploadService,
            IImageProcessingService imageProcessingService)
            : base(categoryUnitOfWork.TableRepository<TbCategory>(), mapper)
        {
            _mapper = mapper;
            _categoryUnitOfWork = categoryUnitOfWork;
            _fileUploadService = fileUploadService;
            _imageProcessingService = imageProcessingService;
        }

        public async Task<PaginatedDataModel<CategoryDto>> GetPageAsync(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter for active entities
            Expression<Func<TbCategory, bool>> filter = x => !x.IsDeleted;

            // Apply search term if provided
            if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
            {
                string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
                filter = x => x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm) ||
                             x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm);
            }

            // Create ordering function based on SortBy and SortDirection
            Func<IQueryable<TbCategory>, IOrderedQueryable<TbCategory>> orderBy = null;

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
                        "createddateutc" => isDescending ? query.OrderByDescending(x => x.CreatedDateUtc) : query.OrderBy(x => x.CreatedDateUtc),
                        "treeviewserial" => isDescending ? query.OrderByDescending(x => x.TreeViewSerial) : query.OrderBy(x => x.TreeViewSerial),
                        _ => query.OrderBy(x => x.CreatedDateUtc) // Default sorting
                    };
                };
            }
            else
            {
                // Default ordering
                orderBy = query => query.OrderBy(x => x.CreatedDateUtc);
            }

            var vwCategoryWithAttributes = await _categoryUnitOfWork.Repository<TbCategory>().GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy: orderBy);

            var categories = _mapper.MapList<TbCategory, CategoryDto>(vwCategoryWithAttributes.Items);

            return new PaginatedDataModel<CategoryDto>(categories, vwCategoryWithAttributes.TotalRecords);
        }
        public override async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryUnitOfWork.Repository<TbCategory>().GetAsync(c => !c.IsDeleted);

            if (categories == null)
                throw new Exception(string.Format(ValidationResources.EntityNotFound, ECommerceResources.Category));
            if (!categories.Any())
                return new List<CategoryDto>();

            return _mapper.MapList<TbCategory, CategoryDto>(categories.OrderBy(c => c.TreeViewSerial, new TreeViewSerialComparer()));
        }

        public override async Task<CategoryDto> FindByIdAsync(Guid Id)
        {
            Expression<Func<VwCategoryWithAttributes, bool>> filter = x => x.Id == Id;
            var vwCategoryWithAttributes = await _categoryUnitOfWork.Repository<VwCategoryWithAttributes>().FindAsync(filter);

            if (vwCategoryWithAttributes == null)
                throw new Exception(string.Format(ValidationResources.EntityNotFound, ECommerceResources.Category));

            return _mapper.MapModel<VwCategoryWithAttributes, CategoryDto>(vwCategoryWithAttributes);
        }
        public async Task<bool> Save(CategoryDto dto, Guid userId)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (userId == Guid.Empty) throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

            try
            {
                await _categoryUnitOfWork.BeginTransactionAsync();
                var allCategories = (await _categoryUnitOfWork.TableRepository<TbCategory>()
                    .GetAsync(c => !c.IsDeleted)).ToList();
                var maxDisplayOrder = allCategories.Any() ? allCategories.Max(c => c.DisplayOrder) : 0;

                // Handle TreeViewSerial (unchanged)
                if (dto.Id == Guid.Empty)
                {
                    // New category - generate TreeViewSerial
                    if (dto.ParentId == Guid.Empty)
                    {
                        // Root category - find max root serial
                        var rootCategories = allCategories.Where(c => c.ParentId == Guid.Empty || c.ParentId == null).ToList();
                        var maxRootSerial = 0;
                        if (rootCategories.Any())
                        {
                            foreach (var root in rootCategories)
                            {
                                if (!string.IsNullOrEmpty(root.TreeViewSerial))
                                {
                                    var parts = root.TreeViewSerial.Split('.');
                                    if (parts.Length > 0 && int.TryParse(parts[0], out var serialNum))
                                    {
                                        if (serialNum > maxRootSerial)
                                            maxRootSerial = serialNum;
                                    }
                                }
                            }
                        }
                        dto.TreeViewSerial = (maxRootSerial + 1).ToString();
                    }
                    else
                    {
                        // Child category - find parent and generate child serial
                        var parent = allCategories.FirstOrDefault(c => c.Id == dto.ParentId);
                        if (parent != null && !string.IsNullOrEmpty(parent.TreeViewSerial))
                        {
                            var parentSerial = parent.TreeViewSerial.TrimEnd('.');
                            var siblings = allCategories
                                .Where(c => c.ParentId == dto.ParentId)
                                .ToList();
                            var maxChildSerial = 0;
                            if (siblings.Any())
                            {
                                foreach (var sibling in siblings)
                                {
                                    if (!string.IsNullOrEmpty(sibling.TreeViewSerial) &&
                                        sibling.TreeViewSerial.StartsWith(parentSerial + "."))
                                    {
                                        var parts = sibling.TreeViewSerial.Split('.');
                                        if (parts.Length > 1 && int.TryParse(parts.Last(), out var childNum))
                                        {
                                            if (childNum > maxChildSerial)
                                                maxChildSerial = childNum;
                                        }
                                    }
                                }
                            }
                            dto.TreeViewSerial = $"{parentSerial}.{maxChildSerial + 1}";
                        }
                        else
                        {
                            // Parent not found or has invalid serial - treat as root
                            var rootCategories = allCategories.Where(c => c.ParentId == Guid.Empty || c.ParentId == null).ToList();
                            var maxRootSerial = 0;
                            if (rootCategories.Any())
                            {
                                foreach (var root in rootCategories)
                                {
                                    if (!string.IsNullOrEmpty(root.TreeViewSerial))
                                    {
                                        var parts = root.TreeViewSerial.Split('.');
                                        if (parts.Length > 0 && int.TryParse(parts[0], out var serialNum))
                                        {
                                            if (serialNum > maxRootSerial)
                                                maxRootSerial = serialNum;
                                        }
                                    }
                                }
                            }
                            dto.TreeViewSerial = (maxRootSerial + 1).ToString();
                        }
                    }
                }
                else
                {
                    // Existing category - check if parent changed
                    var existingCategory = allCategories.FirstOrDefault(c => c.Id == dto.Id);
                    if (existingCategory != null && existingCategory.ParentId != dto.ParentId)
                    {
                        // Parent changed - need to update TreeViewSerial for this category and all descendants
                        if (dto.ParentId == Guid.Empty)
                        {
                            // Moving to root
                            var rootCategories = allCategories
                                .Where(c => (c.ParentId == Guid.Empty || c.ParentId == null) && c.Id != dto.Id)
                                .ToList();
                            var maxRootSerial = 0;
                            if (rootCategories.Any())
                            {
                                foreach (var root in rootCategories)
                                {
                                    if (!string.IsNullOrEmpty(root.TreeViewSerial))
                                    {
                                        var parts = root.TreeViewSerial.Split('.');
                                        if (parts.Length > 0 && int.TryParse(parts[0], out var serialNum))
                                        {
                                            if (serialNum > maxRootSerial)
                                                maxRootSerial = serialNum;
                                        }
                                    }
                                }
                            }
                            dto.TreeViewSerial = (maxRootSerial + 1).ToString();
                        }
                        else
                        {
                            // Moving to new parent
                            var newParent = allCategories.FirstOrDefault(c => c.Id == dto.ParentId);
                            if (newParent != null && !string.IsNullOrEmpty(newParent.TreeViewSerial))
                            {
                                var parentSerial = newParent.TreeViewSerial.TrimEnd('.');
                                var siblings = allCategories
                                    .Where(c => c.ParentId == dto.ParentId && c.Id != dto.Id)
                                    .ToList();
                                var maxChildSerial = 0;
                                if (siblings.Any())
                                {
                                    foreach (var sibling in siblings)
                                    {
                                        if (!string.IsNullOrEmpty(sibling.TreeViewSerial) &&
                                            sibling.TreeViewSerial.StartsWith(parentSerial + "."))
                                        {
                                            var parts = sibling.TreeViewSerial.Split('.');
                                            if (parts.Length > 1 && int.TryParse(parts.Last(), out var childNum))
                                            {
                                                if (childNum > maxChildSerial)
                                                    maxChildSerial = childNum;
                                            }
                                        }
                                    }
                                }
                                dto.TreeViewSerial = $"{parentSerial}.{maxChildSerial + 1}";
                            }
                            else
                            {
                                // New parent not found or has invalid serial - keep existing serial
                                dto.TreeViewSerial = existingCategory.TreeViewSerial;
                            }
                        }

                        // Update descendants TreeViewSerial
                        var descendants = await GetDescendantsAsync(dto.Id);
                        var oldPrefix = existingCategory.TreeViewSerial?.TrimEnd('.');
                        var newPrefix = dto.TreeViewSerial?.TrimEnd('.');
                        if (!string.IsNullOrEmpty(oldPrefix) && !string.IsNullOrEmpty(newPrefix))
                        {
                            foreach (var descendant in descendants)
                            {
                                if (!string.IsNullOrEmpty(descendant.TreeViewSerial) &&
                                    descendant.TreeViewSerial.StartsWith(oldPrefix + "."))
                                {
                                    descendant.TreeViewSerial = descendant.TreeViewSerial.Replace(
                                        oldPrefix + ".", newPrefix + ".");
                                    await _categoryUnitOfWork.TableRepository<TbCategory>().UpdateAsync(descendant, userId);
                                }
                            }
                        }
                    }
                    else
                    {
                        // Parent didn't change - keep existing TreeViewSerial
                        dto.TreeViewSerial = existingCategory?.TreeViewSerial;
                    }
                }

                // Display order handling - FIXED
                if (dto.DisplayOrder <= 0 || dto.DisplayOrder > maxDisplayOrder + 1)
                {
                    dto.DisplayOrder = maxDisplayOrder + 1;
                }

                // Handle file uploads (unchanged)
                if (_fileUploadService.ValidateFile(dto.ImageUrl).isValid)
                    dto.ImageUrl = await SaveImageSync(dto.ImageUrl);
                if (_fileUploadService.ValidateFile(dto.Icon).isValid)
                    dto.Icon = await SaveImageSync(dto.Icon);

                // Entity mapping with display order preservation
                TbCategory entity;
                if (dto.Id == Guid.Empty)
                {
                    // New category - shift display orders if needed
                    if (dto.DisplayOrder <= maxDisplayOrder)
                    {
                        await ShiftDisplayOrderForInsertAsync(dto.DisplayOrder, userId);
                    }

                    entity = _mapper.MapModel<CategoryDto, TbCategory>(dto);
                }
                else
                {
                    // Existing category - preserve display order during mapping
                    var savedCategory = allCategories.FirstOrDefault(c => c.Id == dto.Id);
                    if (savedCategory == null) throw new Exception("Category not found");

                    int originalDisplayOrder = savedCategory.DisplayOrder;
                    savedCategory = _mapper.MapModel<CategoryDto, TbCategory>(dto);
                    savedCategory.DisplayOrder = originalDisplayOrder; // Preserve original order

                    entity = savedCategory;
                }

                // Handle parent relationship changes (unchanged)
                if (dto.ParentId != Guid.Empty)
                {
                    if (dto.Id != Guid.Empty &&
                        entity.ParentId != null &&
                        entity.ParentId != Guid.Empty &&
                        entity.ParentId != dto.ParentId)
                    {
                        await UpdateParentFinalStatus(dto.Id, entity.ParentId.Value, userId, false);
                    }
                    await UpdateParentFinalStatus(dto.Id, dto.ParentId, userId, true);
                    entity.IsFinal = false;
                }
                else
                {
                    entity.IsFinal = true;
                }

                // Save entity
                var result1 = await _categoryUnitOfWork.TableRepository<TbCategory>().SaveAsync(entity, userId);
                var categoryId = result1.Id;

                // Handle display order update for existing categories
                if (dto.Id != Guid.Empty)
                {
                    var savedCategory = allCategories.FirstOrDefault(c => c.Id == dto.Id);
                    if (savedCategory != null && savedCategory.DisplayOrder != dto.DisplayOrder)
                    {
                        await UpdateDisplayOrderAsync(categoryId, dto.DisplayOrder, savedCategory.DisplayOrder, userId, maxDisplayOrder);
                    }
                }

                // Handle attributes (unchanged)
                if (dto.Id != Guid.Empty)
                {
                    var existingAttributes = await _categoryUnitOfWork.TableRepository<TbCategoryAttribute>()
                        .GetAsync(c => c.CategoryId == dto.Id && !c.IsDeleted);
                    if (existingAttributes?.Any() == true)
                    {
                        foreach (var attribute in existingAttributes)
                        {
                            await _categoryUnitOfWork.TableRepository<TbCategoryAttribute>()
                                .HardDeleteAsync(attribute.Id);
                        }
                    }
                }

                if (dto.CategoryAttributes?.Any() == true)
                {
                    foreach (var attribute in dto.CategoryAttributes)
                    {
                        if (attribute.DisplayOrder == 0)
                            attribute.DisplayOrder = dto.CategoryAttributes.Max(a => a.DisplayOrder) + 1;
                        attribute.CategoryId = categoryId;
                    }
                    var categoryAttributes = _mapper.MapList<CategoryAttributeDto, TbCategoryAttribute>(dto.CategoryAttributes);
                    await _categoryUnitOfWork.TableRepository<TbCategoryAttribute>()
                        .AddRangeAsync(categoryAttributes, userId);
                }

                await _categoryUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _categoryUnitOfWork.Rollback();
                throw new Exception(string.Format(ValidationResources.SaveEntityError, ECommerceResources.Category), ex);
            }
        }
        public async Task<bool> UpdateSerialsAsync(Dictionary<Guid, string> serialAssignments, Guid userId)
        {
            try
            {
                if (!serialAssignments?.Any() == true) return true;

                // Convert to the format expected by UpdateBulkFieldWithDifferentValuesAsync
                var entityFieldValues = serialAssignments.ToDictionary(
                    kvp => kvp.Key,
                    kvp => new Dictionary<string, object> { ["TreeViewSerial"] = kvp.Value }
                );

                var result = await _categoryUnitOfWork.TableRepository<TbCategory>().UpdateBulkFieldsAsync(
                    entityFieldValues: entityFieldValues,
                    updaterId: userId
                );

                return result.Success && result.UpdatedCount == serialAssignments.Count;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating category serials: {ex.Message}", ex);
            }
        }
        public async Task<(bool, List<string>)> Delete(Guid id, Guid userId)
        {
            try
            {
                var errors = new List<string>();
                await _categoryUnitOfWork.BeginTransactionAsync();
                var entity = await _categoryUnitOfWork.TableRepository<TbCategory>().FindAsync(x => x.Id == id);
                if (entity == null)
                {
                    _categoryUnitOfWork.Rollback();
                    errors.Add(string.Format(ValidationResources.EntityNotFound, ECommerceResources.Category));
                    throw new ArgumentException(NotifiAndAlertsResources.InvalidInput, nameof(id));
                }
                if (id == Guid.Empty)
                {
                    _categoryUnitOfWork.Rollback();
                    errors.Add(NotifiAndAlertsResources.InvalidInput);
                    throw new ArgumentException(NotifiAndAlertsResources.InvalidInput, nameof(id));
                }

                // Store the TreeViewSerial before deletion for updating siblings
                string deletedTreeViewSerial = entity.TreeViewSerial;
                Guid? parentId = entity.ParentId;

                // Check if the item exists
                var categoryItemsEntities = await _categoryUnitOfWork.TableRepository<TbItem>().GetAsync(i => i.CategoryId == id && !i.IsDeleted);
                var categoryItems = _mapper.MapList<TbItem, ItemDto>(categoryItemsEntities);

                // Prevent deletion if category is in use by items
                if (categoryItems != null && categoryItems.Any())
                {
                    _categoryUnitOfWork.Rollback();
                    var itemTitles = categoryItems.Select(i => i.Title).Take(5).ToList();
                    var formattedTitles = string.Join(", ", itemTitles);
                    string errorMessage;
                    if (itemTitles.Count >= 5)
                    {
                        errorMessage = string.Format(
                            NotifiAndAlertsResources.EntityCannotBeDeletedInUse,
                            ECommerceResources.Category,
                            ECommerceResources.Adverts,
                            formattedTitles
                            ) + "...";
                    }
                    else
                    {
                        errorMessage = string.Format(
                            NotifiAndAlertsResources.EntityCannotBeDeletedInUse,
                            ECommerceResources.Category,
                            ECommerceResources.Adverts,
                            formattedTitles);
                    }
                    errors.Add(errorMessage);
                    return (false, errors);
                }

                var hasChild = (await _categoryUnitOfWork.TableRepository<TbCategory>()
                    .GetAsync(x => x.ParentId == id && !x.IsDeleted)).Any();
                if (hasChild)
                {
                    _categoryUnitOfWork.Rollback();
                    errors.Add(ValidationResources.CategoryHasChild);
                    return (false, errors);
                }

                // Store the display order before deletion for shifting other entities
                int deletedDisplayOrder = entity.DisplayOrder;

                // Reset display order to 0 before deletion
                entity.DisplayOrder = 0;
                await _categoryUnitOfWork.TableRepository<TbCategory>().UpdateAsync(entity, userId);

                // Update current state (soft delete)
                await _categoryUnitOfWork.TableRepository<TbCategory>().UpdateCurrentStateAsync(id, userId, true);

                // Handle category attributes
                var categoryAttributes = (await _categoryUnitOfWork.TableRepository<TbCategoryAttribute>()
                    .GetAsync()).Where(x => x.CategoryId == id);
                if (categoryAttributes?.Count() > 0)
                {
                    foreach (var attribute in categoryAttributes)
                    {
                        await _categoryUnitOfWork.TableRepository<TbCategoryAttribute>().UpdateCurrentStateAsync(attribute.Id, userId, true);
                    }
                }

                // Update TreeViewSerial for siblings after deletion
                if (!string.IsNullOrEmpty(deletedTreeViewSerial))
                {
                    UpdateSiblingsSerialAfterDelete(deletedTreeViewSerial, parentId, userId);
                }

                // Shift display orders of remaining categories
                if (deletedDisplayOrder > 0)
                {
                    await ShiftDisplayOrderAfterDeleteAsync(deletedDisplayOrder, userId);
                }

                await _categoryUnitOfWork.CommitAsync();
                return (true, errors);
            }
            catch (Exception ex)
            {
                _categoryUnitOfWork.Rollback();
                throw new Exception(string.Format(ValidationResources.DeleteEntityError, ECommerceResources.Category), ex);
            }
        }
        public async Task<IEnumerable<MainCategoryDto>> GetMainCategoriesAsync()
        {
            try
            {
                var mainCategories = (await _categoryUnitOfWork.TableRepository<TbCategory>()
                    .GetAsync(c => c.IsMainCategory && !c.IsDeleted)).Take(4);

                if (mainCategories == null || !mainCategories.Any())
                    return new List<MainCategoryDto>();

                return _mapper.MapList<TbCategory, MainCategoryDto>(mainCategories.OrderBy(c => c.TreeViewSerial, new TreeViewSerialComparer()));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ValidationResources.UnexpectedError, ECommerceResources.Category), ex);
            }
        }
        public async Task<IEnumerable<CategoryPreviewDto>> GetPreviewedCategories(bool isFeaturedCategory, bool isParent)
        {
            try
            {
                ////Parents Categotries
                //var categories = _categoryUnitOfWork.TableRepository<TbCategory>()
                //    .Get(c => !c.IsDeleted && (c.ParentId == null || c.ParentId == Guid.Empty) &&
                //        (!isFeaturedCategory || c.IsFeaturedCategory)).ToList();

                Expression<Func<VwCategoryWithAttributes, bool>> filter = x => (!isFeaturedCategory || x.IsFeaturedCategory == true) &&
                  (!isParent || x.ParentId == null || x.ParentId == Guid.Empty);

                var vwCategoryWithAttributes = await _categoryUnitOfWork.Repository<VwCategoryWithAttributes>().GetAsync(filter);

                if (vwCategoryWithAttributes == null)
                    throw new Exception(string.Format(ValidationResources.EntityNotFound, ECommerceResources.Category));

                var categories = _mapper.MapList<VwCategoryWithAttributes, CategoryPreviewDto>(vwCategoryWithAttributes.OrderBy(c => c.TreeViewSerial, new TreeViewSerialComparer()));

                if (categories == null || !categories.Any())
                    return new List<CategoryPreviewDto>();

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ValidationResources.UnexpectedError, ECommerceResources.Category), ex);
            }
        }
        public async Task<IEnumerable<VwCategoryItemsDto>> GetHomeCategories(string userId)
        {
            try
            {

                var vwCategoryWithItems = await _categoryUnitOfWork.Repository<VwCategoryItems>().GetAsync(x => x.IsHomeCategory);

                if (vwCategoryWithItems == null)
                    throw new Exception(string.Format(ValidationResources.EntityNotFound, ECommerceResources.Category));

                var categories = _mapper.MapList<VwCategoryItems, VwCategoryItemsDto>(vwCategoryWithItems);

                if (categories == null || !categories.Any())
                    return new List<VwCategoryItemsDto>();
                foreach (var category in categories)
                {
                    if (category.Items != null && category.Items.Any())
                    {
                        category.Items = category.Items
                            .OrderByDescending(i => i.CreatedDateUtc)
                            .Take(10)
                            .ToList();
                    }
                }
                foreach (var category in categories)
                {
                    foreach (var item in category.Items)
                    {
                        //item.CurrencyInfo = await _userCurrencyService.GetCurrencyInfo(item.ItemCurrencyId, item.Price.Value, item.PriceRequired, userId);
                    }
                }
                return categories.OrderBy(c => c.TreeViewSerial, new TreeViewSerialComparer());
            }
            catch (Exception ex)
            {
                throw new Exception(ValidationResources.UnexpectedError, ex);
            }
        }

        public async Task<List<CategoryTreeDto>> BuildCategoryTree()
        {
            var categoriesWithAttributes = await _categoryUnitOfWork.Repository<VwCategoryWithAttributes>().GetAllAsync();

            if (categoriesWithAttributes == null || !categoriesWithAttributes.Any())
                return new List<CategoryTreeDto>();

            var dtoList = _mapper.MapList<VwCategoryWithAttributes, CategoryDto>(categoriesWithAttributes.OrderBy(c => c.TreeViewSerial, new TreeViewSerialComparer()));

            var dict = dtoList.ToDictionary(c => c.Id, c => new CategoryTreeDto
            {
                TitleEn = c.TitleEn,
                TitleAr = c.TitleAr,
                Id = c.Id,
                ParentId = c.ParentId,
                ImageUrl = c.ImageUrl,
                PriceRequired = c.PriceRequired,
                IsFeaturedCategory = c.IsFeaturedCategory,
                TreeViewSerial = c.TreeViewSerial,
                IsMainCategory = c.IsMainCategory,
                IsHomeCategory = c.IsHomeCategory,
                Icon = c.Icon,
                CategoryAttributes = c.CategoryAttributes,
                Children = new List<CategoryTreeDto>()
            });

            foreach (var item in dtoList)
            {
                if (dict.ContainsKey(item.ParentId))
                {
                    dict[item.ParentId].Children.Add(dict[item.Id]);
                }
            }

            var nonRoots = dict.Values.SelectMany(x => x.Children).ToHashSet();
            return dict.Values.Where(x => !nonRoots.Contains(x)).ToList();
        }
        public async Task<CategoryTreeDto> GetCategoryWithChildren(Guid categoryId)
        {
            try
            {
                var category = await _categoryUnitOfWork.TableRepository<TbCategory>().FindAsync(c => c.Id == categoryId);

                if (category == null)
                    return new CategoryTreeDto();

                var categoryTree = await MapCategoryWithChildren(category);
                if (categoryTree.Children != null && categoryTree.Children.Any())
                    categoryTree.Children = categoryTree.Children.OrderBy(c => c.TreeViewSerial, new TreeViewSerialComparer()).ToList();
                return categoryTree;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ValidationResources.UnexpectedError, ECommerceResources.Category), ex);
            }
        }

        // helpers

        private async Task<List<TbCategory>> GetDescendantsAsync(Guid categoryId)
        {
            var descendants = new List<TbCategory>();
            var stack = new Stack<Guid>();
            stack.Push(categoryId);

            while (stack.Count > 0)
            {
                var currentId = stack.Pop();
                var children = await _categoryUnitOfWork.TableRepository<TbCategory>()
                    .GetAsync(c => c.ParentId == currentId);

                foreach (var child in children)
                {
                    descendants.Add(child);
                    stack.Push(child.Id);
                }
            }

            return descendants;
        }
        private async Task UpdateSiblingsSerialAfterDelete(string deletedSerial, Guid? parentId, Guid userId)
        {
            try
            {
                // Get all active categories
                var allCategories = await _categoryUnitOfWork.TableRepository<TbCategory>()
                    .GetAsync(c => !c.IsDeleted);

                // Determine the parent serial for siblings
                string parentSerial = "";
                if (parentId.HasValue && parentId.Value != Guid.Empty)
                {
                    var parent = allCategories.FirstOrDefault(c => c.Id == parentId.Value);
                    if (parent != null && !string.IsNullOrEmpty(parent.TreeViewSerial))
                    {
                        parentSerial = parent.TreeViewSerial.TrimEnd('.') + ".";
                    }
                }

                // Get siblings that come after the deleted category
                var siblings = allCategories
                    .Where(c => c.ParentId == parentId &&
                                !string.IsNullOrEmpty(c.TreeViewSerial) &&
                                c.TreeViewSerial.StartsWith(parentSerial))
                    .ToList();

                // Parse the deleted serial to get its position
                var deletedParts = deletedSerial.Split('.');
                int deletedLastPart = 0;
                if (deletedParts.Length > 0 && int.TryParse(deletedParts.Last(), out deletedLastPart))
                {
                    // Get siblings with serial numbers greater than the deleted one
                    var siblingsToUpdate = siblings
                        .Where(c =>
                        {
                            var parts = c.TreeViewSerial.Split('.');
                            if (parts.Length > 0 && int.TryParse(parts.Last(), out var lastPart))
                            {
                                return lastPart > deletedLastPart;
                            }
                            return false;
                        })
                        .OrderBy(c => int.Parse(c.TreeViewSerial.Split('.').Last()))
                        .ToList();

                    // Update each sibling's serial
                    foreach (var sibling in siblingsToUpdate)
                    {
                        var parts = sibling.TreeViewSerial.Split('.');
                        if (parts.Length > 0 && int.TryParse(parts.Last(), out var lastPart))
                        {
                            // Remember the old serial before updating
                            string oldSerial = sibling.TreeViewSerial;

                            // Decrement the last part of the serial
                            parts[parts.Length - 1] = (lastPart - 1).ToString();
                            string newSerial = string.Join(".", parts);
                            sibling.TreeViewSerial = newSerial;

                            // Update the sibling in the database
                            await (_categoryUnitOfWork.TableRepository<TbCategory>()).UpdateAsync(sibling, userId);

                            // Update descendants' serials
                            UpdateDescendantsSerials(oldSerial, newSerial, userId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't throw to allow the deletion to continue
                Console.WriteLine($"Error updating siblings serials: {ex.Message}");
            }
        }
        private async Task UpdateDescendantsSerials(string oldParentSerial, string newParentSerial, Guid userId)
        {
            try
            {
                // Get all descendants of the parent
                var descendants = await _categoryUnitOfWork.TableRepository<TbCategory>()
                    .GetAsync(c => c.TreeViewSerial.StartsWith(oldParentSerial + ".") && !c.IsDeleted);

                foreach (var descendant in descendants)
                {
                    // Calculate the new serial by replacing the parent portion
                    var oldSerial = descendant.TreeViewSerial;
                    var suffix = oldSerial.Substring(oldParentSerial.Length);
                    descendant.TreeViewSerial = newParentSerial + suffix;

                    // Update the descendant in the database
                    await _categoryUnitOfWork.TableRepository<TbCategory>().UpdateAsync(descendant, userId);
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't throw to allow the deletion to continue
                Console.WriteLine($"Error updating descendants serials: {ex.Message}");
            }
        }
        private async Task ShiftDisplayOrderAfterDeleteAsync(int deletedDisplayOrder, Guid userId)
        {
            // Get all active categories with display order greater than the deleted category
            var categoriesToShift = await _categoryUnitOfWork.TableRepository<TbCategory>()
                .GetAsync(c => !c.IsDeleted && c.DisplayOrder > deletedDisplayOrder);

            if (!categoriesToShift.Any()) return;

            // Prepare bulk update data
            var updates = new Dictionary<Guid, Dictionary<string, object>>();
            foreach (var category in categoriesToShift)
            {
                updates[category.Id] = new Dictionary<string, object>
                {
                    { "DisplayOrder", category.DisplayOrder - 1 }
                };
            }

            // Execute single bulk update
            await _categoryUnitOfWork.TableRepository<TbCategory>()
                .UpdateBulkFieldsAsync(updates, userId);
        }
        private async Task ShiftDisplayOrderForInsertAsync(int insertPosition, Guid userId)
        {
            // Get all categories at or after the insert position
            var categoriesToShift = await _categoryUnitOfWork.TableRepository<TbCategory>()
                .GetAsync(c => !c.IsDeleted && c.DisplayOrder >= insertPosition);

            if (!categoriesToShift.Any()) return;

            // Prepare bulk update data
            var updates = new Dictionary<Guid, Dictionary<string, object>>();
            foreach (var category in categoriesToShift)
            {
                updates[category.Id] = new Dictionary<string, object>
                {
                    { "DisplayOrder", category.DisplayOrder + 1 }
                };
            }

            // Execute single bulk update
            await _categoryUnitOfWork.TableRepository<TbCategory>()
                .UpdateBulkFieldsAsync(updates, userId);
        }
        private async Task UpdateDisplayOrderAsync(Guid categoryId, int newDisplayOrder, int oldDisplayOrder, Guid userId, int? maxDisplayOrder = null)
        {
            // Validate inputs
            if (newDisplayOrder < 1)
                throw new ArgumentOutOfRangeException(nameof(newDisplayOrder), "DisplayOrder must be positive.");
            if (userId == Guid.Empty)
                throw new ArgumentException("Invalid user ID.", nameof(userId));
            if (oldDisplayOrder == newDisplayOrder)
            {
                // No change needed
                return;
            }

            // Get max display order if not provided
            if (!maxDisplayOrder.HasValue)
            {
                var allCategories = (await _categoryUnitOfWork.TableRepository<TbCategory>()
                    .GetAsync(c => !c.IsDeleted)).ToList();
                maxDisplayOrder = allCategories.Any() ? allCategories.Max(c => c.DisplayOrder) : 0;
            }

            // Clamp newDisplayOrder to valid range
            newDisplayOrder = Math.Max(1, Math.Min(newDisplayOrder, maxDisplayOrder.Value));

            // Get the target category
            var targetCategory = (await _categoryUnitOfWork.TableRepository<TbCategory>()
                .GetAsync(c => c.Id == categoryId)).FirstOrDefault();

            if (targetCategory == null) return;

            // Get all active categories (excluding the target category)
            var categories = (await _categoryUnitOfWork.TableRepository<TbCategory>()
                .GetAsync(c => c.Id != categoryId && !c.IsDeleted)).ToList();

            // Prepare bulk updates
            var updates = new Dictionary<Guid, Dictionary<string, object>>();

            if (newDisplayOrder > oldDisplayOrder)
            {
                // Moving to a higher DisplayOrder: decrement categories in (oldDisplayOrder, newDisplayOrder]
                var categoriesToUpdate = categories
                    .Where(c => c.DisplayOrder > oldDisplayOrder && c.DisplayOrder <= newDisplayOrder)
                    .ToList();

                foreach (var category in categoriesToUpdate)
                {
                    updates[category.Id] = new Dictionary<string, object>
                    {
                        { "DisplayOrder", category.DisplayOrder - 1 }
                    };
                }
            }
            else
            {
                // Moving to a lower DisplayOrder: increment categories in [newDisplayOrder, oldDisplayOrder)
                var categoriesToUpdate = categories
                    .Where(c => c.DisplayOrder >= newDisplayOrder && c.DisplayOrder < oldDisplayOrder)
                    .ToList();

                foreach (var category in categoriesToUpdate)
                {
                    updates[category.Id] = new Dictionary<string, object>
                    {
                        { "DisplayOrder", category.DisplayOrder + 1 }
                    };
                }
            }

            // Add target category update if needed
            if (targetCategory.DisplayOrder != newDisplayOrder)
            {
                updates[targetCategory.Id] = new Dictionary<string, object>
                {
                    { "DisplayOrder", newDisplayOrder }
                };
            }

            // Execute all updates in a single call
            if (updates.Any())
            {
                await _categoryUnitOfWork.TableRepository<TbCategory>()
                    .UpdateBulkFieldsAsync(updates, userId);
            }
        }
        private async Task<CategoryTreeDto> MapCategoryWithChildren(TbCategory category)
        {
            var children = await _categoryUnitOfWork.TableRepository<TbCategory>()
                .GetAsync(c => c.ParentId == category.Id,
                orderBy: q => q.OrderBy(x => x.TreeViewSerial));

            var childTasks = children.Select(child => MapCategoryWithChildren(child));
            var childDtos = (await Task.WhenAll(childTasks)).ToList();

            return new CategoryTreeDto
            {
                Id = category.Id,
                TitleAr = category.TitleAr,
                TitleEn = category.TitleEn,
                ImageUrl = category.ImageUrl,
                TreeViewSerial = category.TreeViewSerial,
                PriceRequired = category.PriceRequired,
                Children = childDtos
            };
        }
        private async Task<string> SaveImageSync(string image)
        {
            // Check if the file is null or empty
            if (image == null || image.Length == 0)
            {
                throw new ValidationException(ValidationResources.ImageRequired);
            }

            // Validate the file
            var imageValidation = _fileUploadService.ValidateFile(image);
            if (!imageValidation.isValid)
            {
                throw new ValidationException(imageValidation.errorMessage);
            }

            try
            {
                // Convert the file to byte array
                var imageBytes = await _fileUploadService.GetFileBytesAsync(image);

                // Resize the image
                var resizedImage = _imageProcessingService.ResizeImage(imageBytes, 1024, 1024);

                // Convert the resized image to WebP format
                var webpImage = _imageProcessingService.ConvertToWebP(resizedImage);

                // Upload the WebP image to the specified location
                var imagePath = await _fileUploadService.UploadFileAsync(webpImage, "Images");

                // Return the path of the uploaded image
                return imagePath;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ValidationResources.ErrorProcessingImage, ex);
            }
        }

        private async Task UpdateParentFinalStatus(Guid childId, Guid parentId, Guid userId, bool isNew)
        {
            var parent = (await _categoryUnitOfWork.TableRepository<TbCategory>()
                .GetAsync(x => x.Id == parentId)).FirstOrDefault()
                ?? throw new Exception(string.Format(ValidationResources.EntityNotFound, ECommerceResources.Category));

            parent.IsFinal = parent.ParentId == null || parent.ParentId == Guid.Empty;

            await _categoryUnitOfWork.TableRepository<TbCategory>().UpdateAsync(parent, userId);
        }
    }
}
