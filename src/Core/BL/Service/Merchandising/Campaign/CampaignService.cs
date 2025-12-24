using AutoMapper;
using BL.Contracts.Service.Merchandising.Campaign;
using DAL.Contracts.Repositories.Merchandising;
using Domains.Entities.Campaign;
using Shared.DTOs.Campaign;

namespace BL.Service.Merchandising.Campaign
{
    /// <summary>
    /// Campaign Service - manages campaigns and flash sales
    /// </summary>
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IMapper _mapper;

        public CampaignService(
            ICampaignRepository campaignRepository,
            IMapper mapper)
        {
            _campaignRepository = campaignRepository;
            _mapper = mapper;
        }

        #region Campaign Management

        /// <summary>
        /// Get campaign by ID
        /// </summary>
        public async Task<CampaignDto?> GetCampaignByIdAsync(Guid id)
        {
            var campaign = await _campaignRepository.GetCampaignByIdAsync(id);
            return campaign != null ? _mapper.Map<CampaignDto>(campaign) : null;
        }

        /// <summary>
        /// Get all active campaigns
        /// </summary>
        public async Task<List<CampaignDto>> GetActiveCampaignsAsync()
        {
            var campaigns = await _campaignRepository.GetActiveCampaignsAsync();
            return _mapper.Map<List<CampaignDto>>(campaigns);
        }

        /// <summary>
        /// Get all active flash sales
        /// </summary>
        public async Task<List<CampaignDto>> GetActiveFlashSalesAsync()
        {
            var flashSales = await _campaignRepository.GetActiveFlashSalesAsync();
            return _mapper.Map<List<CampaignDto>>(flashSales);
        }

        /// <summary>
        /// Create new campaign
        /// </summary>
        public async Task<CampaignDto> CreateCampaignAsync(CreateCampaignDto dto, Guid userId)
        {
            var campaign = _mapper.Map<TbCampaign>(dto);

            // Set defaults
            campaign.Id = Guid.NewGuid();
            campaign.CreatedDateUtc = DateTime.UtcNow;
            campaign.CreatedBy = userId;
            campaign.IsDeleted = false;
            campaign.IsActive = true;

            var result = await _campaignRepository.CreateAsync(campaign, userId);

            if (!result.Success)
            {
                throw new InvalidOperationException("Failed to create campaign");
            }

            var created = await _campaignRepository.GetCampaignByIdAsync(result.Id);
            return _mapper.Map<CampaignDto>(created);
        }

        /// <summary>
        /// Update campaign
        /// </summary>
        public async Task<CampaignDto> UpdateCampaignAsync(UpdateCampaignDto dto, Guid userId)
        {
            var campaign = await _campaignRepository.GetCampaignByIdAsync(dto.Id);
            if (campaign == null)
            {
                throw new KeyNotFoundException($"Campaign with ID {dto.Id} not found");
            }

            // Map updates
            _mapper.Map(dto, campaign);
            campaign.UpdatedDateUtc = DateTime.UtcNow;
            campaign.UpdatedBy = userId;

            var result = await _campaignRepository.UpdateAsync(campaign, userId);

            if (!result.Success)
            {
                throw new InvalidOperationException("Failed to update campaign");
            }

            var updated = await _campaignRepository.GetCampaignByIdAsync(dto.Id);
            return _mapper.Map<CampaignDto>(updated);
        }

        /// <summary>
        /// Delete campaign (soft delete)
        /// </summary>
        public async Task<bool> DeleteCampaignAsync(Guid id, Guid userId)
        {
            // Check if campaign has items
            var items = await _campaignRepository.GetCampaignItemsAsync(id);
            if (items.Any())
            {
                throw new InvalidOperationException("Cannot delete campaign with items. Remove all items first.");
            }

            return await _campaignRepository.SoftDeleteAsync(id, userId);
        }

        #endregion

        #region Campaign Items

        /// <summary>
        /// Get all items in a campaign
        /// </summary>
        public async Task<List<CampaignItemDto>> GetCampaignItemsAsync(Guid campaignId)
        {
            var items = await _campaignRepository.GetCampaignItemsAsync(campaignId);
            return _mapper.Map<List<CampaignItemDto>>(items);
        }

        /// <summary>
        /// Add item to campaign
        /// </summary>
        public async Task<CampaignItemDto> AddItemToCampaignAsync(AddCampaignItemDto dto, Guid userId)
        {
            // Validate campaign exists
            var campaign = await _campaignRepository.GetCampaignByIdAsync(dto.CampaignId);
            if (campaign == null)
            {
                throw new KeyNotFoundException($"Campaign with ID {dto.CampaignId} not found");
            }

            var campaignItem = _mapper.Map<TbCampaignItem>(dto);

            // Set defaults
            campaignItem.Id = Guid.NewGuid();
            campaignItem.CreatedDateUtc = DateTime.UtcNow;
            campaignItem.CreatedBy = userId;
            campaignItem.IsDeleted = false;
            campaignItem.IsActive = true;
            campaignItem.SoldCount = 0;

            var result = await _campaignRepository.AddItemToCampaignAsync(campaignItem);
            return _mapper.Map<CampaignItemDto>(result);
        }

        /// <summary>
        /// Remove item from campaign
        /// </summary>
        public async Task<bool> RemoveItemFromCampaignAsync(Guid campaignItemId, Guid userId)
        {
            return await _campaignRepository.RemoveItemFromCampaignAsync(campaignItemId);
        }

        /// <summary>
        /// Update sold count when item is purchased
        /// </summary>
        public async Task<bool> UpdateSoldCountAsync(Guid campaignItemId, int quantity)
        {
            return await _campaignRepository.IncrementSoldCountAsync(campaignItemId, quantity);
        }

        #endregion

        #region Validation

        /// <summary>
        /// Validate campaign configuration
        /// </summary>
        private void ValidateCampaign(TbCampaign campaign)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(campaign.NameAr))
                errors.Add("Arabic name is required");

            if (string.IsNullOrWhiteSpace(campaign.NameEn))
                errors.Add("English name is required");

            if (campaign.StartDate >= campaign.EndDate)
                errors.Add("End date must be after start date");

            if (campaign.IsFlashSale && !campaign.FlashSaleEndTime.HasValue)
                errors.Add("Flash sale end time is required for flash sales");

            if (errors.Any())
            {
                throw new ArgumentException($"Validation failed: {string.Join(", ", errors)}");
            }
        }

        #endregion
    }
}