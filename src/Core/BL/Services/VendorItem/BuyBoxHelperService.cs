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
using DAL.UnitOfWork;
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
    public class BuyBoxHelperService : IBuyBoxHelperService
    {
        private readonly IBuyBoxHelperRepository _buyBoxHelperRepository;
        private readonly IBaseMapper _mapper;
        private readonly ILogger _logger;
        public BuyBoxHelperService(IBaseMapper mapper,
            ILogger logger,
            IBuyBoxHelperRepository buyBoxHelperRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _buyBoxHelperRepository = buyBoxHelperRepository;
        }

        /// <summary>
        /// Recalculates the Buy Box winners for all combinations of a given item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task RecalculateItemBuyBoxWinnersAsync(Guid itemId)
        {
            try
            {
                    await _buyBoxHelperRepository.RecalculateBuyBoxWinnersByItemIdAsync(itemId);
            }
            catch (Exception ex)
            {
                _logger.Error("Error recalculating By Box winners for ItemId {ItemId}: {ErrorMessage}",
                    itemId, ex.Message);
                throw new Exception("Error while recalculating item By Box winners.");
            }
        }

        /// <summary>
        /// Recalculates the Buy Box winner for all combinations of an item
        /// </summary>
        public async Task RecalculateCombinationBuyBoxWinnerAsync(Guid itemCombinationId)
        {
            try
            {
                await _buyBoxHelperRepository.RecalculateBuyBoxWinnerByItemCombinationIdAsync(itemCombinationId);
            }
            catch (Exception ex)
            {
                _logger.Error("Error recalculating By Box winner for ItemCombinationId {ItemCombinationId}: {ErrorMessage}",
                    itemCombinationId, ex.Message);
                throw new Exception("Error while recalculating item combination By Box winner.");
            }
        }
    }
}