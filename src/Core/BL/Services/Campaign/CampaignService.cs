using BL.Services.Campaign;
using Common.Enumerations.Campaign;
using DAL.ApplicationContext;
using Domains.Entities.Campaign;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Services.Campaign
{
    public class CampaignService : ICampaignService
    {
        private readonly ApplicationDbContext _context;

        public CampaignService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Campaign Management

        public async Task<CampaignDto> GetCampaignByIdAsync(Guid id)
        {
            var campaign = await _context.TbCampaigns
                .Include(c => c.CampaignProducts)
                .Include(c => c.CampaignVendors)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campaign == null) return null;

            return MapToDto(campaign);
        }

        public async Task<List<CampaignDto>> GetAllCampaignsAsync()
        {
            var campaigns = await _context.TbCampaigns
                .Include(c => c.CampaignProducts)
                .Include(c => c.CampaignVendors)
                .OrderByDescending(c => c.CreatedDateUtc)
                .ToListAsync();

            return campaigns.Select(MapToDto).ToList();
        }

        public async Task<List<CampaignDto>> GetActiveCampaignsAsync()
        {
            var now = DateTime.UtcNow;
            var campaigns = await _context.TbCampaigns
                .Include(c => c.CampaignProducts)
                .Include(c => c.CampaignVendors)
                .Where(c => c.IsActive && 
                           c.StartDate <= now && 
                           c.EndDate >= now &&
                           c.Status == CampaignStatus.Active)
                .OrderBy(c => c.StartDate)
                .ToListAsync();

            return campaigns.Select(MapToDto).ToList();
        }

        public async Task<List<CampaignDto>> GetCampaignsByStatusAsync(int status)
        {
            var campaigns = await _context.TbCampaigns
                .Include(c => c.CampaignProducts)
                .Include(c => c.CampaignVendors)
                .Where(c => (int)c.Status == status)
                .OrderByDescending(c => c.CreatedDateUtc)
                .ToListAsync();

            return campaigns.Select(MapToDto).ToList();
        }

        public async Task<CampaignDto> CreateCampaignAsync(CampaignCreateDto dto)
        {
            var campaign = new TbCampaign
            {
                TitleEn = dto.TitleEn,
                TitleAr = dto.TitleAr,
                DescriptionEn = dto.DescriptionEn,
                DescriptionAr = dto.DescriptionAr,
                CampaignType = (CampaignType)dto.CampaignType,
                Status = CampaignStatus.Draft,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                MinimumDiscountPercentage = dto.MinimumDiscountPercentage,
                BudgetLimit = dto.BudgetLimit,
                TotalSpent = 0,
                BannerImagePath = dto.BannerImagePath,
                IsActive = dto.IsActive
            };

            _context.TbCampaigns.Add(campaign);
            await _context.SaveChangesAsync();

            return MapToDto(campaign);
        }

        public async Task<CampaignDto> UpdateCampaignAsync(CampaignUpdateDto dto)
        {
            var campaign = await _context.TbCampaigns.FindAsync(dto.Id);
            if (campaign == null) 
                throw new Exception("Campaign not found");

            campaign.TitleEn = dto.TitleEn;
            campaign.TitleAr = dto.TitleAr;
            campaign.DescriptionEn = dto.DescriptionEn;
            campaign.DescriptionAr = dto.DescriptionAr;
            campaign.CampaignType = (CampaignType)dto.CampaignType;
            campaign.StartDate = dto.StartDate;
            campaign.EndDate = dto.EndDate;
            campaign.MinimumDiscountPercentage = dto.MinimumDiscountPercentage;
            campaign.BudgetLimit = dto.BudgetLimit;
            campaign.BannerImagePath = dto.BannerImagePath;
            campaign.IsActive = dto.IsActive;
            campaign.UpdatedDateUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToDto(campaign);
        }

        public async Task<bool> DeleteCampaignAsync(Guid id)
        {
            var campaign = await _context.TbCampaigns.FindAsync(id);
            if (campaign == null) return false;

            var hasProducts = await _context.TbCampaignProducts
                .AnyAsync(cp => cp.CampaignId == id);
            
            if (hasProducts)
                throw new Exception("Cannot delete campaign with products");

            _context.TbCampaigns.Remove(campaign);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateCampaignAsync(Guid id)
        {
            var campaign = await _context.TbCampaigns.FindAsync(id);
            if (campaign == null) return false;

            campaign.IsActive = true;
            campaign.Status = CampaignStatus.Active;
            campaign.UpdatedDateUtc = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateCampaignAsync(Guid id)
        {
            var campaign = await _context.TbCampaigns.FindAsync(id);
            if (campaign == null) return false;

            campaign.IsActive = false;
            campaign.Status = CampaignStatus.Paused;
            campaign.UpdatedDateUtc = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CampaignDto>> SearchCampaignsAsync(CampaignSearchRequest request)
        {
            var query = _context.TbCampaigns
                .Include(c => c.CampaignProducts)
                .Include(c => c.CampaignVendors)
                .AsQueryable();

            if (request.CampaignType.HasValue)
                query = query.Where(c => (int)c.CampaignType == request.CampaignType.Value);

            if (request.Status.HasValue)
                query = query.Where(c => (int)c.Status == request.Status.Value);

            if (request.IsActive.HasValue)
                query = query.Where(c => c.IsActive == request.IsActive.Value);

            if (request.FromDate.HasValue)
                query = query.Where(c => c.StartDate >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(c => c.EndDate <= request.ToDate.Value);

            var campaigns = await query
                .OrderByDescending(c => c.CreatedDateUtc)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return campaigns.Select(MapToDto).ToList();
        }

        #endregion

        #region Campaign Products

        public async Task<List<CampaignProductDto>> GetCampaignProductsAsync(Guid campaignId)
        {
            var products = await _context.TbCampaignProducts
                .Where(cp => cp.CampaignId == campaignId)
                .OrderBy(cp => cp.DisplayOrder)
                .ToListAsync();

            return products.Select(MapToCampaignProductDto).ToList();
        }

        public async Task<CampaignProductDto> AddProductToCampaignAsync(CampaignProductCreateDto dto)
        {
            var product = new TbCampaignProduct
            {
                CampaignId = dto.CampaignId,
                ItemId = dto.ItemId,
                VendorId = dto.VendorId,
                OriginalPrice = dto.OriginalPrice,
                CampaignPrice = dto.CampaignPrice,
                DiscountPercentage = dto.DiscountPercentage,
                StockQuantity = dto.StockQuantity,
                SoldQuantity = 0,
                DisplayOrder = dto.DisplayOrder,
                IsActive = true
            };

            _context.TbCampaignProducts.Add(product);
            await _context.SaveChangesAsync();

            return MapToCampaignProductDto(product);
        }

        public async Task<bool> RemoveProductFromCampaignAsync(Guid campaignProductId)
        {
            var product = await _context.TbCampaignProducts.FindAsync(campaignProductId);
            if (product == null) return false;

            _context.TbCampaignProducts.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveCampaignProductAsync(Guid campaignProductId, Guid approvedByUserId)
        {
            var product = await _context.TbCampaignProducts.FindAsync(campaignProductId);
            if (product == null) return false;

            product.IsActive = true;
            product.ApprovedByUserId = approvedByUserId;
            product.ApprovedAt = DateTime.UtcNow;
            product.UpdatedDateUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectCampaignProductAsync(Guid campaignProductId)
        {
            var product = await _context.TbCampaignProducts.FindAsync(campaignProductId);
            if (product == null) return false;

            product.IsActive = false;
            product.UpdatedDateUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Flash Sales

        public async Task<List<FlashSaleDto>> GetAllFlashSalesAsync()
        {
            var flashSales = await _context.TbFlashSales
                .Include(fs => fs.FlashSaleProducts)
                .OrderByDescending(fs => fs.CreatedDateUtc)
                .ToListAsync();

            return flashSales.Select(MapToFlashSaleDto).ToList();
        }

        public async Task<List<FlashSaleDto>> GetActiveFlashSalesAsync()
        {
            var now = DateTime.UtcNow;
            var flashSales = await _context.TbFlashSales
                .Include(fs => fs.FlashSaleProducts)
                .Where(fs => fs.IsActive && 
                            fs.StartDate <= now && 
                            fs.EndDate >= now)
                .OrderBy(fs => fs.StartDate)
                .ToListAsync();

            return flashSales.Select(MapToFlashSaleDto).ToList();
        }

        public async Task<FlashSaleDto> GetFlashSaleByIdAsync(Guid id)
        {
            var flashSale = await _context.TbFlashSales
                .Include(fs => fs.FlashSaleProducts)
                .FirstOrDefaultAsync(fs => fs.Id == id);

            if (flashSale == null) return null;

            return MapToFlashSaleDto(flashSale);
        }

        public async Task<FlashSaleDto> CreateFlashSaleAsync(FlashSaleCreateDto dto)
        {
            var flashSale = new TbFlashSale
            {
                TitleEn = dto.TitleEn,
                TitleAr = dto.TitleAr,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                DurationInHours = dto.DurationInHours,
                MinimumDiscountPercentage = dto.MinimumDiscountPercentage,
                BannerImagePath = dto.BannerImagePath,
                ShowCountdownTimer = dto.ShowCountdownTimer,
                IsActive = dto.IsActive,
                TotalSales = 0
            };

            _context.TbFlashSales.Add(flashSale);
            await _context.SaveChangesAsync();

            return MapToFlashSaleDto(flashSale);
        }

        public async Task<FlashSaleDto> UpdateFlashSaleAsync(Guid id, FlashSaleCreateDto dto)
        {
            var flashSale = await _context.TbFlashSales.FindAsync(id);
            if (flashSale == null) 
                throw new Exception("Flash sale not found");

            flashSale.TitleEn = dto.TitleEn;
            flashSale.TitleAr = dto.TitleAr;
            flashSale.StartDate = dto.StartDate;
            flashSale.EndDate = dto.EndDate;
            flashSale.DurationInHours = dto.DurationInHours;
            flashSale.MinimumDiscountPercentage = dto.MinimumDiscountPercentage;
            flashSale.BannerImagePath = dto.BannerImagePath;
            flashSale.ShowCountdownTimer = dto.ShowCountdownTimer;
            flashSale.IsActive = dto.IsActive;
            flashSale.UpdatedDateUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToFlashSaleDto(flashSale);
        }

        public async Task<bool> DeleteFlashSaleAsync(Guid id)
        {
            var flashSale = await _context.TbFlashSales.FindAsync(id);
            if (flashSale == null) return false;

            _context.TbFlashSales.Remove(flashSale);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Statistics & Reports

        public async Task<CampaignStatisticsDto> GetCampaignStatisticsAsync()
        {
            var now = DateTime.UtcNow;

            var totalCampaigns = await _context.TbCampaigns.CountAsync();
            var activeCampaigns = await _context.TbCampaigns
                .CountAsync(c => c.IsActive && c.Status == CampaignStatus.Active);
            var upcomingCampaigns = await _context.TbCampaigns
                .CountAsync(c => c.StartDate > now);
            var expiredCampaigns = await _context.TbCampaigns
                .CountAsync(c => c.EndDate < now);

            var totalRevenue = await _context.TbCampaigns
                .SumAsync(c => c.TotalSpent);

            var totalProductsSold = await _context.TbCampaignProducts
                .SumAsync(cp => cp.SoldQuantity);

            var avgDiscount = await _context.TbCampaigns
                .AverageAsync(c => c.MinimumDiscountPercentage);

            return new CampaignStatisticsDto
            {
                TotalCampaigns = totalCampaigns,
                ActiveCampaigns = activeCampaigns,
                UpcomingCampaigns = upcomingCampaigns,
                ExpiredCampaigns = expiredCampaigns,
                TotalRevenue = totalRevenue,
                TotalProductsSold = totalProductsSold,
                AverageDiscountPercentage = avgDiscount
            };
        }

        public async Task<CampaignStatisticsDto> GetCampaignStatisticsAsync(DateTime fromDate, DateTime toDate)
        {
            var campaigns = await _context.TbCampaigns
                .Where(c => c.StartDate >= fromDate && c.EndDate <= toDate)
                .ToListAsync();

            var activeCampaigns = campaigns.Count(c => c.IsActive && c.Status == CampaignStatus.Active);
            var totalRevenue = campaigns.Sum(c => c.TotalSpent);
            var avgDiscount = campaigns.Any() ? campaigns.Average(c => c.MinimumDiscountPercentage) : 0;

            return new CampaignStatisticsDto
            {
                TotalCampaigns = campaigns.Count,
                ActiveCampaigns = activeCampaigns,
                TotalRevenue = totalRevenue,
                AverageDiscountPercentage = avgDiscount
            };
        }

        public async Task<List<CampaignDto>> GetTopPerformingCampaignsAsync(int count = 10)
        {
            var campaigns = await _context.TbCampaigns
                .Include(c => c.CampaignProducts)
                .OrderByDescending(c => c.TotalSpent)
                .Take(count)
                .ToListAsync();

            return campaigns.Select(MapToDto).ToList();
        }

        #endregion

        #region Helper Methods

        private CampaignDto MapToDto(TbCampaign campaign)
        {
            return new CampaignDto
            {
                Id = campaign.Id,
                TitleEn = campaign.TitleEn,
                TitleAr = campaign.TitleAr,
                DescriptionEn = campaign.DescriptionEn,
                DescriptionAr = campaign.DescriptionAr,
                CampaignType = (int)campaign.CampaignType,
                CampaignTypeName = campaign.CampaignType.ToString(),
                Status = (int)campaign.Status,
                StatusName = campaign.Status.ToString(),
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate,
                MinimumDiscountPercentage = campaign.MinimumDiscountPercentage,
                BudgetLimit = campaign.BudgetLimit,
                TotalSpent = campaign.TotalSpent,
                TotalProducts = campaign.CampaignProducts?.Count ?? 0,
                TotalVendors = campaign.CampaignVendors?.Count ?? 0,
                BannerImagePath = campaign.BannerImagePath,
                IsActive = campaign.IsActive,
                CreatedDateUtc = campaign.CreatedDateUtc
            };
        }

        private CampaignProductDto MapToCampaignProductDto(TbCampaignProduct product)
        {
            return new CampaignProductDto
            {
                Id = product.Id,
                CampaignId = product.CampaignId,
                CampaignTitle = "N/A", // Would need to join
                ItemId = product.ItemId,
                ItemName = "N/A", // Would need to join
                ItemImage = "N/A", // Would need to join
                VendorId = product.VendorId,
                VendorName = "N/A", // Would need to join
                OriginalPrice = product.OriginalPrice,
                CampaignPrice = product.CampaignPrice,
                DiscountPercentage = product.DiscountPercentage,
                StockQuantity = product.StockQuantity,
                SoldQuantity = product.SoldQuantity,
                IsActive = product.IsActive,
                ApprovedAt = product.ApprovedAt,
                ApprovedByUserName = "N/A" // Would need to join
            };
        }

        private FlashSaleDto MapToFlashSaleDto(TbFlashSale flashSale)
        {
            var now = DateTime.UtcNow;
            var timeRemaining = flashSale.EndDate > now 
                ? flashSale.EndDate - now 
                : TimeSpan.Zero;

            return new FlashSaleDto
            {
                Id = flashSale.Id,
                TitleEn = flashSale.TitleEn,
                TitleAr = flashSale.TitleAr,
                StartDate = flashSale.StartDate,
                EndDate = flashSale.EndDate,
                DurationInHours = flashSale.DurationInHours,
                MinimumDiscountPercentage = flashSale.MinimumDiscountPercentage,
                BannerImagePath = flashSale.BannerImagePath,
                TotalProducts = flashSale.FlashSaleProducts?.Count ?? 0,
                TotalSales = flashSale.TotalSales,
                ShowCountdownTimer = flashSale.ShowCountdownTimer,
                IsActive = flashSale.IsActive,
                TimeRemaining = timeRemaining,
                CreatedDateUtc = flashSale.CreatedDateUtc
            };
        }

        #endregion
    }
}
