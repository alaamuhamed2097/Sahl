using BL.Contracts.IMapper;
using BL.Contracts.Service.Catalog.Item;
using DAL.Contracts.Repositories;
using DAL.Models.ItemSearch;
using Domains.Procedures;
using Shared.DTOs.Catalog.Item;
using System.Text.Json;

namespace BL.Service.Catalog.Item
{
    public class ItemDetailsService : IItemDetailsService
    {
        private readonly IItemDetailsRepository _repository;
        private readonly IBaseMapper _mapper;

        public ItemDetailsService(IItemDetailsRepository repository, IBaseMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper;
        }

        public async Task<ItemDetailsDto> GetItemDetailsAsync(Guid itemCombinationId)
        {
            var result = await _repository.GetItemDetailsAsync(itemCombinationId);

            if (result == null)
            {
                throw new KeyNotFoundException($"Item CombinationId with ID {itemCombinationId} not found");
            }

            return _mapper.MapModel<SpGetItemDetails,ItemDetailsDto>(result);
        }

        public async Task<ItemDetailsDto> GetCombinationByAttributesAsync(CombinationRequest request)
        {
            if (request?.SelectedValueIds == null || !request.SelectedValueIds.Any())
            {
                throw new ArgumentException("Selected attributes are required");
            }

            var selections = request.SelectedValueIds
                .Select(a => new AttributeSelection
                {
                    CombinationAttributeValueId = a.CombinationAttributeValueId,
                    IsLastSelected = a.IsLastSelected
                })
                .ToList();

            var result = await _repository.GetCombinationByAttributesAsync(selections);

            if (result == null)
            {
                throw new KeyNotFoundException($"Could not process combination for this selection!!");
            }

            return _mapper.MapModel<SpGetItemDetails, ItemDetailsDto>(result);
        }

        #region Private Mapping Methods



        private CombinationDetailsDto MapCombinationToDto(SpGetAvailableOptionsForSelection result)
        {
            var dto = new CombinationDetailsDto
            {
                CombinationId = result.CombinationId,
                SKU = result.SKU,
                Barcode = result.Barcode,
                IsAvailable = result.IsAvailable,
                Message = result.Message
            };

            // Parse JSON fields
            dto.PricingAttributes = ParseSelectedAttributes(result.SelectedAttributesJson);
            dto.Images = ParseImages(result.ImagesJson);
            dto.Offers = ParseOffers(result.OffersJson);
            dto.Summary = ParseSummary(result.SummaryJson);
            dto.MissingAttributes = ParseMissingAttributes(result.MissingAttributesJson);

            return dto;
        }

        private List<PricingAttributeDto> ParseSelectedAttributes(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<PricingAttributeDto>();

            try
            {
                var attrs = JsonSerializer.Deserialize<List<PricingAttribute>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return attrs?.Select(a => new PricingAttributeDto
                {
                    AttributeId = a.AttributeId,
                    AttributeNameAr = a.AttributeNameAr,
                    AttributeNameEn = a.AttributeNameEn,
                    CombinationValueId = a.CombinationValueId,
                    ValueAr = a.ValueAr,
                    ValueEn = a.ValueEn,
                    IsSelected = a.IsSelected
                }).ToList() ?? new List<PricingAttributeDto>();
            }
            catch
            {
                return new List<PricingAttributeDto>();
            }
        }

        private List<ItemImageDto> ParseImages(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<ItemImageDto>();

            try
            {
                var images = JsonSerializer.Deserialize<List<ItemImage>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return images?.Select(img => new ItemImageDto
                {
                    Path = img.ImageUrl,
                    Order = img.DisplayOrder
                }).ToList() ?? new List<ItemImageDto>();
            }
            catch
            {
                return new List<ItemImageDto>();
            }
        }

        private List<VendorOfferDto> ParseOffers(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<VendorOfferDto>();

            try
            {
                var offers = JsonSerializer.Deserialize<List<VendorOffer>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return offers?.Select(o => new VendorOfferDto
                {
                    OfferId = o.OfferId,
                    VendorId = o.VendorId,
                    VendorName = o.VendorName,
                    VendorNameAr = o.VendorNameAr,
                    VendorRating = o.VendorRating,
                    VendorLogoUrl = o.VendorLogoUrl,
                    Price = o.Price,
                    SalesPrice = o.SalesPrice,
                    DiscountPercentage = o.DiscountPercentage,
                    AvailableQuantity = o.AvailableQuantity,
                    StockStatus = o.StockStatus,
                    IsFreeShipping = o.IsFreeShipping,
                    ShippingCost = o.ShippingCost,
                    EstimatedDeliveryDays = o.EstimatedDeliveryDays,
                    IsBuyBoxWinner = o.IsBuyBoxWinner,
                    HasWarranty = o.HasWarranty,
                    ConditionNameAr = o.ConditionNameAr,
                    ConditionNameEn = o.ConditionNameEn,
                    WarrantyTypeAr = o.WarrantyTypeAr,
                    WarrantyTypeEn = o.WarrantyTypeEn,
                    WarrantyPeriodMonths = o.WarrantyPeriodMonths,
                    MinOrderQuantity = o.MinOrderQuantity,
                    MaxOrderQuantity = o.MaxOrderQuantity,
                    OfferRank = o.OfferRank,
                    QuantityTiers = o.QuantityTiers?.Select(qt => new QuantityTierDto
                    {
                        MinQuantity = qt.MinQuantity,
                        MaxQuantity = qt.MaxQuantity,
                        UnitPrice = qt.UnitPrice
                    }).ToList()
                }).ToList() ?? new List<VendorOfferDto>();
            }
            catch
            {
                return new List<VendorOfferDto>();
            }
        }

        private CombinationSummaryDto ParseSummary(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                var summary = JsonSerializer.Deserialize<CombinationSummary>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (summary == null)
                    return null;

                return new CombinationSummaryDto
                {
                    TotalVendors = summary.TotalVendors,
                    IsMultiVendor = summary.IsMultiVendor,
                    MinPrice = summary.MinPrice,
                    MaxPrice = summary.MaxPrice,
                    AvgPrice = summary.AvgPrice,
                    TotalStock = summary.TotalStock
                };
            }
            catch
            {
                return null;
            }
        }

        private List<MissingAttributeDto> ParseMissingAttributes(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<MissingAttributeDto>();

            try
            {
                var missing = JsonSerializer.Deserialize<List<MissingAttribute>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return missing?.Select(m => new MissingAttributeDto
                {
                    AttributeId = m.AttributeId,
                    NameAr = m.NameAr,
                    NameEn = m.NameEn,
                    Status = m.Status
                }).ToList() ?? new List<MissingAttributeDto>();
            }
            catch
            {
                return new List<MissingAttributeDto>();
            }
        }

        #endregion
    }
}