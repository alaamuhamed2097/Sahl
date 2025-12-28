using BL.Contracts.IMapper;
using BL.Contracts.Service.Catalog.Item;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Customer;
using DAL.Models.ItemSearch;
using DAL.Repositories.Customer;
using Domains.Entities.ECommerceSystem.Customer;
using Domains.Procedures;
using Shared.DTOs.Catalog.Item;
using System.Text.Json;

namespace BL.Service.Catalog.Item
{
    public class ItemDetailsService : IItemDetailsService
    {
        private readonly IItemDetailsRepository _repository;
        private readonly ICustomerItemViewRepository _customerItemViewRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBaseMapper _mapper;

        public ItemDetailsService(IItemDetailsRepository repository, IBaseMapper mapper, ICustomerItemViewRepository customerItemViewRepository, ICustomerRepository customerRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper;
            _customerItemViewRepository = customerItemViewRepository;
            _customerRepository = customerRepository;
        }

        public async Task<ItemDetailsDto> GetItemDetailsAsync(Guid itemCombinationId, string? viewerId)
        {
            // Get item details 
            var result = await _repository.GetItemDetailsAsync(itemCombinationId);
            // If not found, throw exception
            if (result == null)
            {
                throw new KeyNotFoundException($"Item CombinationId with ID {itemCombinationId} not found");
            }
            // Mapp to DTO
            var itemDetailsDto = _mapper.MapModel<SpGetItemDetails, ItemDetailsDto>(result);

            // If viewerId is provided, Create customer view
            if (ShouldRecordView(viewerId, itemDetailsDto))
            {
                await RecordCustomerViewAsync(itemCombinationId, viewerId);
            }

            return itemDetailsDto;
        }

        public async Task<ItemDetailsDto> GetCombinationByAttributesAsync(CombinationRequest request, string? viewerId)
        {
            // Validate input
            if (request?.SelectedValueIds == null || !request.SelectedValueIds.Any())
            {
                throw new ArgumentException("Selected attributes are required");
            }
            // Map selections
            var selections = request.SelectedValueIds
                .Select(a => new AttributeSelection
                {
                    CombinationAttributeValueId = a.CombinationAttributeValueId,
                    IsLastSelected = a.IsLastSelected
                })
                .ToList();
            // Get combination by attributes
            var result = await _repository.GetCombinationByAttributesAsync(selections);
            // If not found, throw exception
            if (result == null)
            {
                throw new KeyNotFoundException($"Could not process combination for this selection!!");
            }
            // Map to DTO
            var itemDetailsDto = _mapper.MapModel<SpGetItemDetails, ItemDetailsDto>(result);

            // If viewerId is provided, Create customer view
            if (ShouldRecordView(viewerId, itemDetailsDto))
            {
                await RecordCustomerViewAsync(itemDetailsDto.CurrentCombination.CombinationId, viewerId);
            }
            return itemDetailsDto;
        }

        // helper methods

        private static bool ShouldRecordView(string? viewerId, ItemDetailsDto itemDetailsDto)
        {
            if (string.IsNullOrEmpty(viewerId))
            {
                return false;
            }

            if (!Guid.TryParse(viewerId, out var viewerGuid))
            {
                return false;
            }

            // Don't record views from the item creator
            return itemDetailsDto.CurrentCombination?.CreatedBy != viewerGuid;
        }

        private async Task RecordCustomerViewAsync(Guid itemCombinationId, string? viewerId)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(viewerId) || itemCombinationId == Guid.Empty )
                return;
            // Get and validate customer
            var customer = await GetAndValidateCustomerAsync(viewerId);

            if (await HasRecentViewAsync(customer.Id, itemCombinationId))
            {
                return;
            }

            await CreateCustomerViewAsync(itemCombinationId, customer.Id, viewerId);
        }

        private async Task<TbCustomer> GetAndValidateCustomerAsync(string viewerId)
        {
            var customer = await _customerRepository.GetCustomerByUserIdAsync(viewerId);

            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found");
            }

            return customer;
        }

        private async Task<bool> HasRecentViewAsync(Guid customerId, Guid itemCombinationId)
        {
            var cutoffTime = DateTime.UtcNow.AddHours(-24);
            var recentViews = await _customerItemViewRepository.GetAsync(
                v => v.CustomerId == customerId &&
                     v.ItemCombinationId == itemCombinationId &&
                     !v.IsDeleted &&
                     v.ViewedAt >= cutoffTime);

            return recentViews.Any();
        }

        private async Task CreateCustomerViewAsync(Guid itemCombinationId, Guid customerId, string viewerId)
        {
            var customerItemView = new TbCustomerItemView
            {
                ItemCombinationId = itemCombinationId,
                CustomerId = customerId,
                ViewedAt = DateTime.UtcNow
            };

            await _customerItemViewRepository.CreateAsync(customerItemView, Guid.Parse(viewerId));
        }
    }
}