using BL.Contracts.IMapper;
using BL.Contracts.Service.ECommerce.Item;
using BL.Contracts.Service.PromoCode;
using Common.Enumerations;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using Domains.Entities.PromoCode;
using Microsoft.Extensions.Logging;
using Resources;
using Shared.DTOs.ECommerce.Order;
using Shared.DTOs.ECommerce.PromoCode;
using Shared.GeneralModels.Parameters;
using Shared.GeneralModels.ResultModels;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Service.PromoCode
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemService _itemService;
        private readonly IBaseMapper _mapper;
        private readonly ILogger<PromoCodeService> _logger;

        public PromoCodeService(IUnitOfWork unitOfWork,
            IItemService itemService,
            IBaseMapper mapper,
            ILogger<PromoCodeService> logger)
        {
            _unitOfWork = unitOfWork;
            _itemService = itemService;
            _mapper = mapper;
            _logger = logger;
        }

        public PaginatedDataModel<PromoCodeDto> GetPage(BaseSearchCriteriaModel criteriaModel)
        {
            if (criteriaModel == null)
                throw new ArgumentNullException(nameof(criteriaModel));

            if (criteriaModel.PageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

            if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
                throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

            // Base filter for active entities
            Expression<Func<TbPromoCode, bool>> filter = x => x.CurrentState == 1;

            // Apply search term if provided
            if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
            {
                string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
                filter = x => x.CurrentState == 1 &&
                              (x.TitleAR != null && x.TitleAR.ToLower().Contains(searchTerm) ||
                               x.TitleEN != null && x.TitleEN.ToLower().Contains(searchTerm) ||
                               x.Code != null && x.Code.ToLower().Contains(searchTerm));
            }

            var items = _unitOfWork.TableRepository<TbPromoCode>().GetPage(
                criteriaModel.PageNumber,
                criteriaModel.PageSize,
                filter,
                orderBy: i => i.OrderByDescending(q => q.CreatedDateUtc));

            var itemsDto = _mapper.MapList<TbPromoCode, PromoCodeDto>(items.Items);
            foreach (var item in itemsDto)
            {
                item.IsActive = IsActivePromoCode(item);
            }

            return new PaginatedDataModel<PromoCodeDto>(itemsDto, items.TotalRecords);
        }

        public List<PromoCodeDto> GetAll()
        {
            var promoCodes = _unitOfWork.TableRepository<TbPromoCode>()
                .Get(predicate: p => p.CurrentState == 1)
                .ToList();

            var itemsDto = _mapper.MapList<TbPromoCode, PromoCodeDto>(promoCodes).ToList();
            foreach (var item in itemsDto)
            {
                item.IsActive = IsActivePromoCode(item);
            }

            return itemsDto;
        }

        public PromoCodeDto GetById(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("Invalid promo code ID");

            var promo = _unitOfWork.TableRepository<TbPromoCode>()
                .Get(
                    predicate: p => p.CurrentState == 1 && p.Id == id,
                    includeProperties: "Orders")
                .FirstOrDefault();

            if (promo == null)
                throw new KeyNotFoundException($"Promo code with ID {id} not found");

            var itemDto = _mapper.MapModel<TbPromoCode, PromoCodeDto>(promo);
            itemDto.IsActive = IsActivePromoCode(itemDto);
            return itemDto;
        }

        public async Task<bool> Save(PromoCodeDto dto, Guid userId)
        {
            ValidateSaveParameters(dto, userId);
            ValidatePromoCodeType(dto);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                SavePromoCodeEntity(dto, userId);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving promo code");
                _unitOfWork.Rollback();
                throw;
            }
        }

        private void ValidateSaveParameters(PromoCodeDto dto, Guid userId)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (userId == Guid.Empty) throw new ArgumentException("Invalid user ID");
        }

        private void ValidatePromoCodeType(PromoCodeDto dto)
        {
            if (dto.PromoCodeType == PromoCodeType.Percentage)
            {
                // Validate percentage value (should be between 0 and 100)
                if (dto.Value <= 0 || dto.Value > 100)
                {
                    throw new ArgumentException("Percentage value must be between 0 and 100");
                }
            }
            else if (dto.PromoCodeType == PromoCodeType.FixedValue)
            {
                // Validate fixed value (should be positive)
                if (dto.Value <= 0)
                {
                    throw new ArgumentException("Fixed value must be greater than 0");
                }
            }
        }

        private Guid SavePromoCodeEntity(PromoCodeDto dto, Guid userId)
        {
            var entity = _mapper.MapModel<PromoCodeDto, TbPromoCode>(dto);

            if (entity.Id != Guid.Empty)
            {
                var exists = _unitOfWork.TableRepository<TbPromoCode>().Get(p => p.Id == entity.Id && p.CurrentState == 0).Any();
                if (exists)
                    throw new ArgumentException("Invalid promo code ID");
            }
            else
            {
                var codeExists = _unitOfWork.TableRepository<TbPromoCode>().IsExists("Code", dto.Code);
                if (codeExists)
                    throw new ArgumentException("The code you entered already exists. Please enter a unique code.");
            }

            _unitOfWork.TableRepository<TbPromoCode>().Save(entity, userId, out Guid promoCodeId);
            return promoCodeId;
        }

        public bool Delete(Guid id, Guid userId)
        {
            if (id == Guid.Empty) throw new ArgumentException("Invalid ID");

            try
            {
                return _unitOfWork.TableRepository<TbPromoCode>().UpdateCurrentState(id, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting promo code {id}");
                throw;
            }
        }

        public async Task<ServiceResult<AppliedPromoCodeResult>> ApplyPromoCode(ApplyPromoCodeRequest request)
        {
            try
            {
                // Validate the promo code
                var validationResult = await ValidatePromoCodeAsync(request.Code, request.UserId);

                // First get all prices from the database
                var priceLookupResult = GetPricesForCartItems(request.OrderItems);
                if (!priceLookupResult.Success)
                    return ServiceResult<AppliedPromoCodeResult>.FailureResult(priceLookupResult.Message);

                var itemPrices = priceLookupResult.Data;
                decimal cartTotal = itemPrices.Sum(x => x.Price * x.Quantity);

                if (!validationResult.Success || !validationResult.Data.IsValid)
                    return ServiceResult<AppliedPromoCodeResult>.FailureResult(validationResult.Message);

                // Get the full promo code entity
                var promoCode = _unitOfWork.TableRepository<TbPromoCode>()
                    .Get(p => p.Id == validationResult.Data.PromoCodeId && p.CurrentState == 1)
                    .FirstOrDefault();

                if (promoCode == null)
                    return ServiceResult<AppliedPromoCodeResult>.FailureResult("Promo code not found");

                decimal totalDiscount = 0;

                // Calculate cart-wide discount
                totalDiscount = CalculateCartDiscount(_mapper.MapModel<TbPromoCode, PromoCodeDto>(promoCode), cartTotal);

                // Update usage count
                promoCode.UsageCount++;
                _unitOfWork.TableRepository<TbPromoCode>().Save(promoCode, Guid.Empty);

                try
                {
                    await _unitOfWork.CommitAsync();
                }
                catch
                {
                    _unitOfWork.Rollback();
                    throw;
                }

                var result = new AppliedPromoCodeResult
                {
                    PromoCodeId = promoCode.Id,
                    Code = promoCode.Code,
                    TotalDiscount = totalDiscount,
                    DiscountType = promoCode.PromoCodeType,
                    NewTotal = cartTotal - totalDiscount,
                    OriginalTotal = cartTotal
                };

                return ServiceResult<AppliedPromoCodeResult>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error applying promo code {request.Code}");
                return ServiceResult<AppliedPromoCodeResult>.FailureResult("Error applying promo code");
            }
        }

        public async Task<ServiceResult<PromoCodeValidationResult>> ValidatePromoCodeAsync(string code, string userId)
        {
            try
            {
                // Get active promo code
                var promoCode = _unitOfWork.TableRepository<TbPromoCode>()
                    .Get(predicate: p => p.Code == code && p.CurrentState == 1,
                         includeProperties: "Orders").FirstOrDefault();

                if (promoCode == null)
                    return ServiceResult<PromoCodeValidationResult>.FailureResult("Invalid promo code");

                var promoCodeDto = _mapper.MapModel<TbPromoCode, PromoCodeDto>(promoCode);

                // Check date validity
                if (DateTime.UtcNow < promoCodeDto.StartDateUTC)
                    return ServiceResult<PromoCodeValidationResult>.FailureResult("Promo code not yet active");

                if (DateTime.UtcNow > promoCodeDto.EndDateUTC)
                    return ServiceResult<PromoCodeValidationResult>.FailureResult("Promo code expired");

                // Check usage limits
                if (promoCodeDto.UsageLimit > 0 && promoCodeDto.UsageCount >= promoCodeDto.UsageLimit)
                    return ServiceResult<PromoCodeValidationResult>.FailureResult("Promo code usage limit reached");

                //// Check the user is marketer
                //if (!await _marketerService.IsMarketerAsync(userId))
                //    return ServiceResult<PromoCodeValidationResult>.FailureResult("Promo code for marketers only");

                return ValidateCartPromo(promoCodeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error validating promo code {code}");
                return ServiceResult<PromoCodeValidationResult>.FailureResult("Error validating promo code");
            }
        }

        private bool IsActivePromoCode(PromoCodeDto promoCode)
        {
            if (promoCode.StartDateUTC > DateTime.UtcNow)
                return false;

            if (promoCode.EndDateUTC <= DateTime.UtcNow)
                return false;

            if (promoCode.UsageCount >= promoCode.UsageLimit)
                return false;

            return true;
        }

        private ServiceResult<List<OrderItemPriceDto>> GetPricesForCartItems(IEnumerable<OrderItemDto> orderItems)
        {
            var result = new List<OrderItemPriceDto>();

            try
            {
                foreach (var item in orderItems)
                {
                    var product = _itemService.FindById(item.Id);

                    if (product == null)
                        return ServiceResult<List<OrderItemPriceDto>>.FailureResult($"Product {item.Id} not found");

                    // Use GetPrice() method instead of Price property (now handled by combinations)
                    decimal price = product.GetPrice();

                    result.Add(new OrderItemPriceDto
                    {
                        ItemId = item.Id,
                        Price = price,
                        Quantity = item.Quantity
                    });
                }

                return ServiceResult<List<OrderItemPriceDto>>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching prices for cart items");
                return ServiceResult<List<OrderItemPriceDto>>.FailureResult("Error fetching item prices");
            }
        }

        private ServiceResult<PromoCodeValidationResult> ValidateCartPromo(PromoCodeDto promoCode)
        {
            var result = new PromoCodeValidationResult
            {
                PromoCodeId = promoCode.Id,
                Code = promoCode.Code,
                DiscountValue = promoCode.Value,
                DiscountType = promoCode.PromoCodeType,
                IsValid = true,
                ApplicableItems = new List<Guid>() // All items are applicable for cart-wide discounts
            };

            return ServiceResult<PromoCodeValidationResult>.SuccessResult(result);
        }

        private decimal CalculateCartDiscount(PromoCodeDto promoCode, decimal cartTotal)
        {
            return promoCode.PromoCodeType == PromoCodeType.Percentage
                ? cartTotal * (promoCode.Value / 100)
                : Math.Min(promoCode.Value, cartTotal);
        }
    }
}
