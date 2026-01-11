using BL.Contracts.IMapper;
using BL.Contracts.Service.Customer;
using Common.Filters;
using DAL.Contracts.Repositories.Customer;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Customer;
using Microsoft.EntityFrameworkCore;
using Resources;
using Serilog;
using Shared.DTOs.Customer;
using System.Linq.Expressions;

namespace BL.Services.Customer
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
    }
}