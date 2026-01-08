using BL.Contracts.IMapper;
using BL.Contracts.Service.Catalog.Item;
using DAL.Contracts.Repositories.Catalog.Item;
using DAL.Contracts.Repositories.Customer;
using DAL.Models.ItemSearch;
using Domains.Entities.ECommerceSystem.Customer;
using Domains.Procedures;
using Serilog;
using Shared.DTOs.Catalog.Item;

namespace BL.Services.Catalog.Item
{
    public class ItemDetailsService : IItemDetailsService
    {
        private readonly IItemDetailsRepository _repository;
        private readonly ICustomerItemViewRepository _customerItemViewRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBaseMapper _mapper;
        private readonly ILogger _logger;

        public ItemDetailsService(
            IItemDetailsRepository repository,
            IBaseMapper mapper,
            ICustomerItemViewRepository customerItemViewRepository,
            ICustomerRepository customerRepository,
            ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _customerItemViewRepository = customerItemViewRepository ?? throw new ArgumentNullException(nameof(customerItemViewRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<ItemDetailsDto> GetItemDetailsAsync(
            Guid itemCombinationId,
            string? viewerId,
            CancellationToken cancellationToken = default)
        {

            if (itemCombinationId == Guid.Empty)
            {
                throw new ArgumentException("ItemCombinationId cannot be empty", nameof(itemCombinationId));
            }

            // Get item details with cancellation token
            var result = await _repository.GetItemDetailsAsync(itemCombinationId, cancellationToken);

            // Map to DTO
            var itemDetailsDto = _mapper.MapModel<SpGetItemDetails, ItemDetailsDto>(result);

            if (itemDetailsDto == null)
            {
                throw new InvalidOperationException("Failed to map item details");
            }

            // If viewerId is provided, record customer view
            if (ShouldRecordView(viewerId, itemDetailsDto))
            {
                // Don't let view recording failure break the request
                try
                {
                    await RecordCustomerViewAsync(itemCombinationId, viewerId, cancellationToken);
                }
                catch (Exception ex)
                {
                    // Log the error but don't fail the request
                    _logger.Warning(ex, "Failed to record customer view for {ItemCombinationId}", itemCombinationId);
                }
            }

            return itemDetailsDto;
        }

        public async Task<ItemDetailsDto> GetCombinationByAttributesAsync(
            CombinationRequest request,
            string? viewerId,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.SelectedValueIds == null || !request.SelectedValueIds.Any())
            {
                throw new ArgumentException("At least one attribute must be selected", nameof(request));
            }

            // Map selections
            var selections = request.SelectedValueIds
                .Select(a => new AttributeSelection
                {
                    CombinationAttributeValueId = a.CombinationAttributeValueId,
                    IsLastSelected = a.IsLastSelected
                })
                .ToList();

            // Get combination by attributes with cancellation token
            var result = await _repository.GetCombinationByAttributesAsync(selections, cancellationToken);

            // Map to DTO
            var itemDetailsDto = _mapper.MapModel<SpGetItemDetails, ItemDetailsDto>(result);

            if (itemDetailsDto == null)
            {
                throw new InvalidOperationException("Failed to map combination details");
            }

            // If viewerId is provided, record customer view
            if (ShouldRecordView(viewerId, itemDetailsDto))
            {
                try
                {
                    var combinationId = itemDetailsDto.CurrentCombination?.CombinationId ?? Guid.Empty;
                    if (combinationId != Guid.Empty)
                    {
                        await RecordCustomerViewAsync(combinationId, viewerId, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    // Log error but don't fail the request
                    _logger.Warning(ex, "Failed to record customer view");
                }
            }

            return itemDetailsDto;
        }

        // ============================================
        // Helper Methods
        // ============================================

        private static bool ShouldRecordView(string? viewerId, ItemDetailsDto itemDetailsDto)
        {
            if (string.IsNullOrWhiteSpace(viewerId))
            {
                return false;
            }

            if (!Guid.TryParse(viewerId, out var viewerGuid))
            {
                return false;
            }

            if (itemDetailsDto?.CurrentCombination == null)
            {
                return false;
            }

            // Don't record views from the item creator
            return itemDetailsDto.CurrentCombination.CreatedBy != viewerGuid;
        }

        private async Task RecordCustomerViewAsync(
            Guid itemCombinationId,
            string? viewerId,
            CancellationToken cancellationToken = default)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(viewerId) || itemCombinationId == Guid.Empty)
            {
                return;
            }

            // Get and validate customer
            var customer = await GetAndValidateCustomerAsync(viewerId, cancellationToken);
            if (customer == null)
            {
                return; // Customer not found, skip view recording
            }

            // Check if already viewed recently
            if (await HasRecentViewAsync(customer.Id, itemCombinationId, cancellationToken))
            {
                return;
            }

            // Create the view record
            await CreateCustomerViewAsync(itemCombinationId, customer.Id, viewerId, cancellationToken);
        }

        private async Task<TbCustomer?> GetAndValidateCustomerAsync(
            string viewerId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByUserIdAsync(viewerId);
                return customer;
            }
            catch (Exception)
            {
                // If customer not found, return null
                // Don't throw - view recording is not critical
                return null;
            }
        }

        private async Task<bool> HasRecentViewAsync(
            Guid customerId,
            Guid itemCombinationId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // ✅ Consider making this configurable
                const int ViewWindowHours = 24;
                var cutoffTime = DateTime.UtcNow.AddHours(-ViewWindowHours);

                var recentViews = await _customerItemViewRepository.GetAsync(
                    v => v.CustomerId == customerId &&
                         v.ItemCombinationId == itemCombinationId &&
                         !v.IsDeleted &&
                         v.ViewedAt >= cutoffTime);

                return recentViews.Any();
            }
            catch (Exception)
            {
                // If check fails, assume no recent view to allow recording
                return false;
            }
        }

        private async Task CreateCustomerViewAsync(
            Guid itemCombinationId,
            Guid customerId,
            string viewerId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var customerItemView = new TbCustomerItemView
                {
                    ItemCombinationId = itemCombinationId,
                    CustomerId = customerId,
                    ViewedAt = DateTime.UtcNow
                };

                await _customerItemViewRepository.CreateAsync(customerItemView, Guid.Parse(viewerId));
            }
            catch (Exception ex)
            {
                _logger.Warning(ex, "Failed to record customer view for {ItemCombinationId}", itemCombinationId);
            }
        }
    }
}