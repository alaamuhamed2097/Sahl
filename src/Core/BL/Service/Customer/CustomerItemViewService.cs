using BL.Contracts.IMapper;
using BL.Contracts.Service.Customer;
using BL.Contracts.Service.Customer.Wishlist;
using BL.Contracts.Service.Order;
using Common.Enumerations.Offer;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Customer;
using DAL.Exceptions;
using DAL.Models;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.Merchandising;
using Domains.Entities.Offer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resources;
using Serilog;
using Shared.DTOs.Catalog.Item;
using Shared.DTOs.Customer;
using Shared.DTOs.Customer.Wishlist;
using Shared.DTOs.ECommerce.Cart;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Globalization;
using System.Linq.Expressions;

namespace BL.Service.Customer.Wishlist
{
    /// <summary>
    /// Service for managing customer Item Views
    /// Uses custom repository for customer-scoped operations
    /// </summary>
    public class CustomerItemViewService : ICustomerItemViewService
    {
        private readonly ICustomerItemViewRepository _customerItemViewRepository;
        private readonly ILogger _logger;
        private readonly IBaseMapper _mapper;

        public CustomerItemViewService(

            ILogger logger, ICustomerItemViewRepository customerItemViewRepository, IBaseMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _customerItemViewRepository = customerItemViewRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<CustomerItemViewDto>> GetPage(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter for active entities
            Expression<Func<TbCustomerItemView, bool>> filter = x => !x.IsDeleted;

            // Apply search term if provided
            if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
            {
                string searchTerm = criteriaModel.SearchTerm.Trim();

                filter = x => x.ItemCombination.Item.TitleAr != null && EF.Functions.Like(x.ItemCombination.Item.TitleAr, $"%{searchTerm}%") ||
                              x.ItemCombination.Item.TitleEn != null && EF.Functions.Like(x.ItemCombination.Item.TitleEn, $"%{searchTerm}%") ||
                              x.ItemCombination.Item.ShortDescriptionAr != null && EF.Functions.Like(x.ItemCombination.Item.ShortDescriptionAr, $"%{searchTerm}%") ||
                              x.ItemCombination.Item.ShortDescriptionEn != null && EF.Functions.Like(x.ItemCombination.Item.ShortDescriptionEn, $"%{searchTerm}%") ||
                              x.ItemCombination.Item.DescriptionAr != null && EF.Functions.Like(x.ItemCombination.Item.DescriptionAr, $"%{searchTerm}%") ||
                              x.ItemCombination.Item.DescriptionEn != null && EF.Functions.Like(x.ItemCombination.Item.DescriptionEn, $"%{searchTerm}%") ||
                              x.ItemCombination.Item.Brand.NameAr != null && EF.Functions.Like(x.ItemCombination.Item.Brand.NameAr, $"%{searchTerm}%") ||
                              x.ItemCombination.Item.Brand.NameEn != null && EF.Functions.Like(x.ItemCombination.Item.Brand.NameEn, $"%{searchTerm}%");
            }

            var entitiesList = await _customerItemViewRepository.GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter);

            var dtoList = _mapper.MapList<TbCustomerItemView, CustomerItemViewDto>(entitiesList.Items);

            return new PagedResult<CustomerItemViewDto>(dtoList, entitiesList.TotalRecords);
        }

        public async Task<bool> AddCustomerItemViewAsync(CustomerItemViewDto customerItemViewDto, Guid creatorId)
        {
            if (customerItemViewDto == null)
                throw new ArgumentNullException(nameof(customerItemViewDto));
            var entity = _mapper.MapModel<CustomerItemViewDto, TbCustomerItemView>(customerItemViewDto);
            await _customerItemViewRepository.CreateAsync(entity, creatorId);
            return true;
        }
        //public async Task<IEnumerable<ItemSectionDto>> GetRecommendedItemsByUserId(ItemFilterDto criteriaModel, Guid userId)
        //{
        //    // Validation
        //    if (userId == Guid.Empty)
        //        throw new Exception("User not found");
        //    ValidateCriteria(criteriaModel);

        //    // Handle category + subcategories
        //    var subCategoryIds = new List<Guid>();
        //    if (criteriaModel.CategoryId != null && criteriaModel.CategoryId != Guid.Empty)
        //        subCategoryIds = await GetAllSubCategoryIds(criteriaModel.CategoryId.Value);
        //    criteriaModel.CategoryIds = subCategoryIds;

        //    var filter = await BuildFilteredQuery(criteriaModel, userId.ToString());
        //    var recommendedItems = await _userViewUnitOfWork.Repository<VwRecommendedAdsForUser>().GetAsync(filter);

        //    if (recommendedItems == null || !recommendedItems.Any())
        //        return new List<ItemSectionDto>();

        //    var itemDtos = _mapper.MapList<VwRecommendedAdsForUser, ItemSectionDto>(recommendedItems).ToList();

        //    var currencyInput = itemDtos.Select(i => (i.ItemCurrencyId, i.Price, i.PriceRequired)).ToList();
        //    var currencyInfoList = await _userCurrencyService.GetCurrencyInfoForItems(currencyInput, userId.ToString());

        //    for (int i = 0; i < itemDtos.Count(); i++)
        //    {
        //        itemDtos[i].CurrencyInfo = currencyInfoList[i];
        //        itemDtos[i].IsFavorite = !string.IsNullOrEmpty(userId.ToString()) || userId != Guid.Empty
        //        ? (await _userFavoriteItem
        //        .GetAsync(x => x.ItemId == itemDtos[i].Id && x.UserId == userId.ToString() && x.CurrentState == 1)).Any()
        //        : false;
        //    }

        //    // Sorting
        //    if (criteriaModel.Popular)
        //        itemDtos = itemDtos.OrderByDescending(x => x.ViewCount).ToList();
        //    else if (criteriaModel.Latest)
        //        itemDtos = itemDtos.OrderByDescending(x => x.CreatedDateUtc).ToList();
        //    else if (criteriaModel.Price == 1)
        //        itemDtos = itemDtos.OrderBy(x => x.CurrencyInfo.ConvertedPrice).ToList();
        //    else if (criteriaModel.Price == 2)
        //        itemDtos = itemDtos.OrderByDescending(x => x.CurrencyInfo.ConvertedPrice).ToList();

        //    return itemDtos;
        //}

        //helper methods
        //    private void ValidateCriteria(BaseSearchCriteriaModel criteria)
        //    {
        //        if (criteria == null)
        //            throw new ArgumentNullException(nameof(criteria));

        //        if (criteria.PageNumber < 1)
        //            throw new ArgumentOutOfRangeException(nameof(criteria.PageNumber), ValidationResources.PageNumberGreaterThanZero);

        //        if (criteria.PageSize < 1 || criteria.PageSize > 100)
        //            throw new ArgumentOutOfRangeException(nameof(criteria.PageSize), ValidationResources.PageSizeRange);
        //    }
        //    private async Task<List<Guid>> GetAllSubCategoryIds(Guid rootCategoryId)
        //    {
        //        var allCategories = await _categoryRepository
        //            .GetAsync(x => x.ParentId.HasValue && x.ParentId != Guid.Empty); // Only get categories that have parents

        //        var parentToChildrenMap = allCategories
        //            .GroupBy(c => c.ParentId.Value)
        //            .ToDictionary(g => g.Key, g => g.Select(c => c.Id).ToList());

        //        var result = new List<Guid>();
        //        var currentLevelIds = new List<Guid> { rootCategoryId };

        //        while (currentLevelIds.Any())
        //        {
        //            var nextLevelIds = new List<Guid>();

        //            foreach (var parentId in currentLevelIds)
        //            {
        //                if (parentToChildrenMap.TryGetValue(parentId, out var childIds))
        //                {
        //                    result.AddRange(childIds);
        //                    nextLevelIds.AddRange(childIds);
        //                }
        //            }

        //            currentLevelIds = nextLevelIds;
        //        }
        //        if (result.Any())
        //            result.Add(rootCategoryId);
        //        return result;
        //    }
        //    private async Task<Expression<Func<VwRecommendedAdsForUser, bool>>> BuildFilteredQuery(ExtendedSearchCriteriaModel criteria, string? userId)
        //    {
        //        Expression<Func<VwRecommendedAdsForUser, bool>> filter = x => x.UserId == userId.ToString();

        //        var viewer = await _userManager.FindByIdAsync(userId);
        //        Guid viewerCurrencyId = viewer?.CurrencyId ?? await _currencyService.GetDefaultCurrencyId();
        //        var viewerCurrency = await _currencyRepository.FindAsync(x => x.Id == viewerCurrencyId);

        //        if (viewerCurrency == null)
        //        {
        //            return filter = x => false; // No valid currency, return empty result
        //            //return query.Where(x => false); // return empty query safely
        //        }

        //        if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
        //        {
        //            var term = criteria.SearchTerm.Trim().ToLower();
        //            filter = filter.And(x =>
        //                x.Title.ToLower().Contains(term));
        //        }

        //        if (criteria.CategoryIds?.Any() == true)
        //            filter = filter.And(x => criteria.CategoryIds.Contains(x.CategoryId));
        //        else if (criteria.CategoryId.HasValue)
        //            filter = filter.And(x => x.CategoryId == criteria.CategoryId.Value);

        //        // Attributes (requires materialization, stays as is for now)
        //        if (criteria.CategoryAttributes?.Any() == true)
        //        {
        //            foreach (var attr in criteria.CategoryAttributes)
        //            {
        //                if (attr.AttributeId != Guid.Empty)
        //                {
        //                    var attrId = attr.AttributeId;
        //                    if (attr.IsRangeFieldType)
        //                    {
        //                        filter = await HandleRangeFieldType(filter, attr, attrId);
        //                    }
        //                    else
        //                    {
        //                        var value = attr.Value?.Trim().ToLower();

        //                        if (!string.IsNullOrEmpty(value))
        //                        {
        //                            var itemIds = (await _itemAttributeRepository
        //                                                 .GetAsync(a => a.AttributeId == attrId && a.Value.ToLower().Contains(value)))
        //                                                 .Select(a => a.ItemId)
        //                                                 .Distinct()
        //                                                 .ToList();
        //                            if (itemIds.Any())
        //                                filter = filter.And(x => itemIds.Contains(x.ItemId));
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        // Price filter
        //        if (criteria.PriceFrom.HasValue || criteria.PriceTo.HasValue)
        //        {
        //            if (criteria.PriceFrom != 0 || criteria.PriceTo != 0)
        //            {
        //                var viewerRate = viewerCurrency.ConversionRate;

        //                if (criteria.PriceFrom.HasValue)
        //                {
        //                    filter = filter.And(item =>
        //                        (item.Price / item.ConversionRate) * viewerRate >= criteria.PriceFrom.Value);
        //                }

        //                if (criteria.PriceTo.HasValue)
        //                {
        //                    filter = filter.And(item =>
        //                        (item.Price / item.ConversionRate) * viewerRate <= criteria.PriceTo.Value);
        //                }
        //            }
        //        }

        //        // Location
        //        if (criteria.Latitude.HasValue && criteria.Longitude.HasValue && criteria.UserLocation)
        //        {
        //            criteria.Latitude = TruncateDecimal(criteria.Latitude.Value, 6);
        //            criteria.Longitude = TruncateDecimal(criteria.Longitude.Value, 6);
        //            criteria.RadiusKm = (criteria.RadiusKm > 0) ? criteria.RadiusKm.Value : 100;
        //            filter = ApplyLocationFilter(filter, criteria.Latitude.Value, criteria.Longitude.Value, criteria.RadiusKm.Value);
        //        }
        //        else if (viewer != null && criteria.UserLocation &&
        //            (viewer.CurrentLatitude != default && viewer.CurrentLongitude != default))
        //        {
        //            filter = ApplyLocationFilter(filter, viewer.CurrentLatitude, viewer.CurrentLongitude);
        //        }

        //        return filter;
        //    }
        //    private Expression<Func<VwRecommendedAdsForUser, bool>> ApplyLocationFilter(
        //Expression<Func<VwRecommendedAdsForUser, bool>> filter,
        //decimal latitude,
        //decimal longitude,
        //decimal radiusKm = 100)
        //    {
        //        if (latitude < -90 || latitude > 90)
        //            throw new ArgumentException("Latitude must be between -90 and 90");
        //        if (longitude < -180 || longitude > 180)
        //            throw new ArgumentException("Longitude must be between -180 and 180");

        //        decimal calcLatitude = Math.Min(Math.Abs(latitude), 85m);

        //        decimal radius = radiusKm;
        //        decimal latDelta = radius / 111m;
        //        decimal lngDelta = radius / (111m * (decimal)Math.Cos(Deg2Rad((double)calcLatitude)));

        //        decimal minLat = Math.Max(-90m, latitude - latDelta);
        //        decimal maxLat = Math.Min(90m, latitude + latDelta);
        //        decimal minLng = longitude - lngDelta;
        //        decimal maxLng = longitude + lngDelta;

        //        // Handle date line crossing BEFORE truncation
        //        if (minLng < -180 || maxLng > 180)
        //        {
        //            minLng = NormalizeLongitude(minLng);
        //            maxLng = NormalizeLongitude(maxLng);

        //            if (minLng > maxLng) // Crosses date line
        //            {
        //                // Truncate AFTER normalization
        //                minLng = TruncateDecimal(minLng, 6);
        //                maxLng = TruncateDecimal(maxLng, 6);
        //                decimal truncMinLat = TruncateDecimal(minLat, 6);
        //                decimal truncMaxLat = TruncateDecimal(maxLat, 6);

        //                filter = filter.And(x =>
        //                    x.Latitude >= truncMinLat && x.Latitude <= truncMaxLat &&
        //                    (x.Longitude >= minLng || x.Longitude <= maxLng));
        //                return filter;
        //            }
        //        }

        //        // Truncate final values
        //        minLat = TruncateDecimal(minLat, 6);
        //        maxLat = TruncateDecimal(maxLat, 6);
        //        minLng = TruncateDecimal(minLng, 6);
        //        maxLng = TruncateDecimal(maxLng, 6);

        //        filter = filter.And(x =>
        //            x.Latitude >= minLat && x.Latitude <= maxLat &&
        //            x.Longitude >= minLng && x.Longitude <= maxLng);

        //        return filter;
        //    }

        //private decimal TruncateDecimal(decimal value, int precision)
        //{
        //    decimal multiplier = (decimal)Math.Pow(10, precision);
        //    return Math.Truncate(value * multiplier) / multiplier;
        //}

        //private decimal NormalizeLongitude(decimal lng)
        //{
        //    // Handle values outside [-180, 180] range
        //    while (lng > 180) lng -= 360;
        //    while (lng < -180) lng += 360;
        //    return lng;
        //}

        //private double Deg2Rad(double deg) => deg * (Math.PI / 180);

        //private async Task<Expression<Func<VwRecommendedAdsForUser, bool>>> HandleRangeFieldType(Expression<Func<VwRecommendedAdsForUser, bool>> filter, SearchAttributeFilterDto attr, Guid attrId)
        //{
        //    // Handle range attributes
        //    var rangeValues = attr.Value?.Split('-').Select(v => v.Trim()).ToList();

        //    // Handle edge cases for range values
        //    decimal decimalFrom = 0, decimalTo = 0;
        //    DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MaxValue;
        //    bool isDecimalRange = false, isDateRange = false;

        //    // Case 1: "-value" format (from 0 to value)
        //    if (attr.Value?.StartsWith("-") == true && rangeValues?.Count == 2 && string.IsNullOrEmpty(rangeValues[0]))
        //    {
        //        if (decimal.TryParse(rangeValues[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var maxValue))
        //        {
        //            decimalFrom = 0;
        //            decimalTo = maxValue;
        //            isDecimalRange = true;
        //        }
        //        else if (DateTime.TryParse(rangeValues[1], CultureInfo.InvariantCulture, DateTimeStyles.None, out var maxDate))
        //        {
        //            dateFrom = DateTime.MinValue;
        //            dateTo = maxDate;
        //            isDateRange = true;
        //        }
        //    }
        //    // Case 2: "value-" format (from value to infinity/max)
        //    else if (attr.Value?.EndsWith("-") == true && rangeValues?.Count == 2 && string.IsNullOrEmpty(rangeValues[1]))
        //    {
        //        if (decimal.TryParse(rangeValues[0], NumberStyles.Number, CultureInfo.InvariantCulture, out var minValue))
        //        {
        //            decimalFrom = minValue;
        //            decimalTo = decimal.MaxValue;
        //            isDecimalRange = true;
        //        }
        //        else if (DateTime.TryParse(rangeValues[0], CultureInfo.InvariantCulture, DateTimeStyles.None, out var minDate))
        //        {
        //            dateFrom = minDate;
        //            dateTo = DateTime.MaxValue;
        //            isDateRange = true;
        //        }
        //    }
        //    // Case 3: Normal "value-value" format
        //    else if (rangeValues?.Count == 2)
        //    {
        //        // Try decimal parsing first
        //        if (decimal.TryParse(rangeValues[0], NumberStyles.Number, CultureInfo.InvariantCulture, out decimalFrom) &&
        //            decimal.TryParse(rangeValues[1], NumberStyles.Number, CultureInfo.InvariantCulture, out decimalTo))
        //        {
        //            isDecimalRange = true;
        //        }
        //        // Try DateTime parsing
        //        else if (DateTime.TryParse(rangeValues[0], CultureInfo.InvariantCulture, DateTimeStyles.None, out dateFrom) &&
        //                 DateTime.TryParse(rangeValues[1], CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTo))
        //        {
        //            isDateRange = true;
        //        }
        //    }

        //    // Apply decimal range filter
        //    if (isDecimalRange)
        //    {
        //        var attributesInRange = await _itemAttributeRepository
        //            .GetAsync(a => a.AttributeId == attrId);

        //        List<Guid> itemIds = attributesInRange
        //            .Where(a => decimal.TryParse(a.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
        //            .Where(a => {
        //                decimal.TryParse(a.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out var value);
        //                // Handle "value-" case where decimalTo might be MaxValue
        //                if (decimalTo == decimal.MaxValue)
        //                    return value >= decimalFrom;
        //                else
        //                    return value >= decimalFrom && value <= decimalTo;
        //            })
        //            .Select(a => a.ItemId)
        //            .Distinct()
        //            .ToList();

        //        filter = filter.And(x => itemIds.Contains(x.ItemId));
        //    }
        //    // Apply DateTime range filter
        //    else if (isDateRange)
        //    {
        //        var attributesInRange = await _itemAttributeRepository
        //            .GetAsync(a => a.AttributeId == attrId);

        //        var itemIds = attributesInRange
        //            .Where(a => DateTime.TryParse(a.Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue))
        //            .Where(a => {
        //                DateTime.TryParse(a.Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue);
        //                // Handle "value-" case where dateTo might be MaxValue
        //                if (dateTo == DateTime.MaxValue)
        //                    return dateValue >= dateFrom;
        //                else
        //                    return dateValue >= dateFrom && dateValue <= dateTo;
        //            })
        //            .Select(a => a.ItemId)
        //            .Distinct()
        //            .ToList();

        //        filter = filter.And(x => itemIds.Contains(x.ItemId));
        //    }

        //    return filter;
        //}
    }
}