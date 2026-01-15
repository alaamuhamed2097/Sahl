using BL.Contracts.GeneralService.CMS;
using BL.Contracts.IMapper;
using BL.Contracts.Service.VendorItem;
using BL.Extensions;
using Common.Enumerations.Offer;
using Common.Enumerations.Pricing;
using Common.Enumerations.Visibility;
using Common.Filters;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Offer;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Offer;
using DAL.ResultModels;
using Domains.Entities.Catalog.Category;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Offer;
using Domains.Views.Offer;
using Microsoft.EntityFrameworkCore;
using Resources;
using Serilog;
using Shared.DTOs.Catalog.Item;
using Shared.DTOs.ECommerce.Offer;
using Shared.GeneralModels.SearchCriteriaModels;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace BL.Services.VendorItem
{
    public class VendorItemConditionService : IVendorItemConditionService
    {
        private readonly IVendorItemConditionRepository _vendorItemConditionRepository;
        private readonly IBaseMapper _mapper;
        private readonly ILogger _logger;
        public VendorItemConditionService(IBaseMapper mapper,
            ILogger logger,
            IVendorItemConditionRepository vendorItemConditionRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _vendorItemConditionRepository = vendorItemConditionRepository;
        }

        public async Task<PagedResult<OfferConditionDto>> GetPage(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter
            Expression<Func<TbOfferCondition, bool>> filter = x => !x.IsDeleted;

            // Combine expressions manually
            var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();

            // Get paginated data from repository
            var offerConditions = await _vendorItemConditionRepository.GetPageAsync(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy: q => q.OrderByDescending(x => x.CreatedDateUtc)
            );

            var offerConditionsDto = _mapper.MapList<TbOfferCondition, OfferConditionDto>(offerConditions.Items);

            return new PagedResult<OfferConditionDto>(offerConditionsDto, offerConditions.TotalRecords);
        }
        public async Task<IEnumerable<OfferConditionDto>> GetNewConditions()
        {
            var conditions = await _vendorItemConditionRepository.GetAsync(vo=>vo.IsNew);
            return _mapper.MapList<TbOfferCondition, OfferConditionDto>(conditions);
        }

        public async Task<OfferConditionDto> FindByIdAsync(Guid Id)
        {
            if (Id == Guid.Empty)
                throw new ArgumentNullException(nameof(Id));

            var offerConditions = await _vendorItemConditionRepository.GetAsync(
                x => x.Id == Id,
                orderBy: i => i.OrderByDescending(x => x.CreatedDateUtc)
            );

            var offerCondition = offerConditions.FirstOrDefault();
            if (offerCondition == null)
                throw new KeyNotFoundException(ValidationResources.EntityNotFound);

            return _mapper.MapModel<TbOfferCondition, OfferConditionDto>(offerCondition);
        }
        public async Task<SaveResult> SaveAsync(OfferConditionDto dto, Guid userId)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));
                if (userId == Guid.Empty)
                    throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

                var entity = _mapper.MapModel<OfferConditionDto, TbOfferCondition>(dto);
                // Save offer condition entity
                var offerConditionSaved = await _vendorItemConditionRepository.SaveAsync(entity, userId);

                return offerConditionSaved;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error saving offerCondition {ItemId}", dto.Id);
                throw;
            }
        }
        public async Task<bool> DeleteAsync(Guid offerConditionId, Guid userId)
        {
            try
            {
                // Validate inputs
                if (offerConditionId == Guid.Empty)
                    throw new ArgumentException(ValidationResources.EntityNotFound, "Offer Condition");
                if (userId == Guid.Empty)
                    throw new ArgumentException(UserResources.UserNotFound, nameof(userId));

                // Soft Delete offer condition entity
                var offerConditionSaved = await _vendorItemConditionRepository.UpdateIsDeletedAsync(offerConditionId, userId);

                return offerConditionSaved;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleteing offerCondition {offerConditionId}", offerConditionId);
                throw;
            }
        }
    }
}