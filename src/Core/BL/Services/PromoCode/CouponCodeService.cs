using BL.Contracts.IMapper;
using BL.Contracts.Service.Catalog.Item;
using BL.Contracts.Service.CouponCode;
using Common.Enumerations;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using Domains.Entities.CouponCode;
using Resources;
using Serilog;
using Shared.DTOs.Order.CouponCode;
using Shared.GeneralModels.ResultModels;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Services.PromoCode;

public class CouponCodeService : ICouponCodeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IItemService _itemService;
    private readonly IBaseMapper _mapper;
    private readonly ILogger _logger;

    public CouponCodeService(IUnitOfWork unitOfWork,
        IItemService itemService,
        IBaseMapper mapper,
        ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _itemService = itemService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<CouponCodeDto>> GetPage(BaseSearchCriteriaModel criteriaModel)
    {
        if (criteriaModel == null)
            throw new ArgumentNullException(nameof(criteriaModel));

        if (criteriaModel.PageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

        if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
            throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

        // Base filter for active entities
        Expression<Func<TbCouponCode, bool>> filter = x => !x.IsDeleted;

        // Apply search term if provided
        if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
        {
            string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
            filter = x => !x.IsDeleted &&
                          (x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm) ||
                           x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm) ||
                           x.Code != null && x.Code.ToLower().Contains(searchTerm));
        }

        var items = await _unitOfWork.TableRepository<TbCouponCode>().GetPageAsync(
            criteriaModel.PageNumber,
            criteriaModel.PageSize,
            filter,
            orderBy: i => i.OrderByDescending(q => q.CreatedDateUtc));

        var itemsDto = _mapper.MapList<TbCouponCode, CouponCodeDto>(items.Items);
        foreach (var item in itemsDto)
        {
            item.IsActive = IsActiveCouponCode(item);
        }

        return new PagedResult<CouponCodeDto>(itemsDto, items.TotalRecords);
    }

    public async Task<List<CouponCodeDto>> GetAll()
    {
        var couponCodes = await _unitOfWork.TableRepository<TbCouponCode>()
            .GetAllAsync();

        var itemsDto = _mapper.MapList<TbCouponCode, CouponCodeDto>(couponCodes).ToList();
        foreach (var item in itemsDto)
        {
            item.IsActive = IsActiveCouponCode(item);
        }

        return itemsDto;
    }

    public async Task<CouponCodeDto> GetById(Guid id)
    {
        if (id == Guid.Empty) throw new ArgumentException("Invalid promo code ID");

        var promo = (await _unitOfWork.TableRepository<TbCouponCode>()
            .GetAsync(
                predicate: p => !p.IsDeleted && p.Id == id,
                includeProperties: "Orders"))
            .FirstOrDefault();

        if (promo == null)
            throw new KeyNotFoundException($"Promo code with ID {id} not found");

        var itemDto = _mapper.MapModel<TbCouponCode, CouponCodeDto>(promo);
        itemDto.IsActive = IsActiveCouponCode(itemDto);
        return itemDto;
    }

    public async Task<bool> Save(CouponCodeDto dto, Guid userId)
    {
        ValidateSaveParameters(dto, userId);
        ValidateCouponCodeType(dto);

        try
        {
            await _unitOfWork.BeginTransactionAsync();

            await SaveCouponCodeEntityAsync(dto, userId);
            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error saving promo code");
            _unitOfWork.RollbackAsync();
            throw;
        }
    }

    private void ValidateSaveParameters(CouponCodeDto dto, Guid userId)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        if (userId == Guid.Empty) throw new ArgumentException("Invalid user ID");
    }

    private void ValidateCouponCodeType(CouponCodeDto dto)
    {
        if (dto.CouponCodeType == CouponCodeType.Percentage)
        {
            // Validate percentage value (should be between 0 and 100)
            if (dto.Value <= 0 || dto.Value > 100)
            {
                throw new ArgumentException("Percentage value must be between 0 and 100");
            }
        }
        else if (dto.CouponCodeType == CouponCodeType.FixedValue)
        {
            // Validate fixed value (should be positive)
            if (dto.Value <= 0)
            {
                throw new ArgumentException("Fixed value must be greater than 0");
            }
        }
    }

    private async Task<Guid> SaveCouponCodeEntityAsync(CouponCodeDto dto, Guid userId)
    {
        var entity = _mapper.MapModel<CouponCodeDto, TbCouponCode>(dto);

        if (entity.Id != Guid.Empty)
        {
            var exists = (await _unitOfWork.TableRepository<TbCouponCode>().GetAsync(p => p.Id == entity.Id && !p.IsDeleted)).Any();
            if (exists)
                throw new ArgumentException("Invalid promo code ID");
        }
        else
        {
            var codeExists = await _unitOfWork.TableRepository<TbCouponCode>().IsExistsAsync("Code", dto.Code);
            if (codeExists)
                throw new ArgumentException("The code you entered already exists. Please enter a unique code.");
        }

        var saveResult = await _unitOfWork.TableRepository<TbCouponCode>().SaveAsync(entity, userId);
        return saveResult.Id;
    }

    public async Task<bool> Delete(Guid id, Guid userId)
    {
        if (id == Guid.Empty) throw new ArgumentException("Invalid ID");

        try
        {
            return await _unitOfWork.TableRepository<TbCouponCode>().UpdateCurrentStateAsync(id, userId);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error deleting promo code {id}");
            throw;
        }
    }

    //public async Task<ServiceResult<AppliedCouponCodeResult>> ApplyCouponCode(ApplyCouponCodeRequest request)
    //{
    //    try
    //    {
    //        // Validate the promo code
    //        var validationResult = await ValidateCouponCodeAsync(request.Code, request.UserId);

    //        // First get all prices from the database
    //        var priceLookupResult = await GetPricesForCartItemsAsync(request.OrderItems);
    //        if (!priceLookupResult.Success)
    //            return ServiceResult<AppliedCouponCodeResult>.FailureResult(priceLookupResult.Message);

    //        var itemPrices = priceLookupResult.Data;
    //        decimal cartTotal = itemPrices.Sum(x => x.Price * x.Quantity);

    //        if (!validationResult.Success || !validationResult.Data.IsValid)
    //            return ServiceResult<AppliedCouponCodeResult>.FailureResult(validationResult.Message);

    //        // Get the full promo code entity
    //        var couponCode = (await _unitOfWork.TableRepository<TbCouponCode>()
    //            .GetAsync(p => p.Id == validationResult.Data.CouponCodeId && !p.IsDeleted))
    //            .FirstOrDefault();

    //        if (couponCode == null)
    //            return ServiceResult<AppliedCouponCodeResult>.FailureResult("Promo code not found");

    //        decimal totalDiscount = 0;

    //        // Calculate cart-wide discount
    //        totalDiscount = CalculateCartDiscount(_mapper.MapModel<TbCouponCode, CouponCodeDto>(couponCode), cartTotal);

    //        // Update usage count
    //        couponCode.UsageCount++;
    //        await _unitOfWork.TableRepository<TbCouponCode>().SaveAsync(couponCode, Guid.Empty);

    //        try
    //        {
    //            await _unitOfWork.CommitAsync();
    //        }
    //        catch
    //        {
    //            _unitOfWork.Rollback();
    //            throw;
    //        }

    //        var result = new AppliedCouponCodeResult
    //        {
    //            CouponCodeId = couponCode.Id,
    //            Code = couponCode.Code,
    //            TotalDiscount = totalDiscount,
    //            DiscountType = couponCode.CouponCodeType,
    //            NewTotal = cartTotal - totalDiscount,
    //            OriginalTotal = cartTotal
    //        };

    //        return ServiceResult<AppliedCouponCodeResult>.SuccessResult(result);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error(ex, $"Error applying promo code {request.Code}");
    //        return ServiceResult<AppliedCouponCodeResult>.FailureResult("Error applying promo code");
    //    }
    //}

    public async Task<ServiceResult<CouponCodeValidationResult>> ValidateCouponCodeAsync(string code, string userId)
    {
        try
        {
            // Get active promo code
            var couponCode = (await _unitOfWork.TableRepository<TbCouponCode>()
                .GetAsync(predicate: p => p.Code == code && !p.IsDeleted,
                     includeProperties: "Orders")).FirstOrDefault();

            if (couponCode == null)
                return ServiceResult<CouponCodeValidationResult>.FailureResult("Invalid promo code");

            var couponCodeDto = _mapper.MapModel<TbCouponCode, CouponCodeDto>(couponCode);

            // Check date validity
            if (DateTime.UtcNow < couponCodeDto.StartDateUTC)
                return ServiceResult<CouponCodeValidationResult>.FailureResult("Promo code not yet active");

            if (DateTime.UtcNow > couponCodeDto.EndDateUTC)
                return ServiceResult<CouponCodeValidationResult>.FailureResult("Promo code expired");

            // Check usage limits
            if (couponCodeDto.UsageLimit > 0 && couponCodeDto.UsageCount >= couponCodeDto.UsageLimit)
                return ServiceResult<CouponCodeValidationResult>.FailureResult("Promo code usage limit reached");

            //// Check the user is marketer
            //if (!await _marketer_service.IsMarketerAsync(userId))
            //    return ServiceResult<CouponCodeValidationResult>.FailureResult("Promo code for marketers only");

            return ValidateCartPromo(couponCodeDto);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error validating promo code {code}");
            return ServiceResult<CouponCodeValidationResult>.FailureResult("Error validating promo code");
        }
    }

    private bool IsActiveCouponCode(CouponCodeDto couponCode)
    {
        if (couponCode.StartDateUTC > DateTime.UtcNow)
            return false;

        if (couponCode.EndDateUTC <= DateTime.UtcNow)
            return false;

        if (couponCode.UsageCount >= couponCode.UsageLimit)
            return false;

        return true;
    }

    //private async Task<ServiceResult<List<OrderItemPriceDto>>> GetPricesForCartItemsAsync(IEnumerable<OrderItemDto> orderItems)
    //{
    //    var result = new List<OrderItemPriceDto>();

    //    try
    //    {
    //        foreach (var item in orderItems)
    //        {
    //            var product = await _itemService.FindByIdAsync(item.Id);

    //            if (product == null)
    //                return ServiceResult<List<OrderItemPriceDto>>.FailureResult($"Product {item.Id} not found");

    //            // Use GetPrice() method instead of Price property (now handled by combinations)
    //            decimal price = product.GetPrice();

    //            result.Add(new OrderItemPriceDto
    //            {
    //                ItemId = item.Id,
    //                Price = price,
    //                Quantity = item.Quantity
    //            });
    //        }

    //        return ServiceResult<List<OrderItemPriceDto>>.SuccessResult(result);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error(ex, "Error fetching prices for cart items");
    //        return ServiceResult<List<OrderItemPriceDto>>.FailureResult("Error fetching item prices");
    //    }
    //}

    private ServiceResult<CouponCodeValidationResult> ValidateCartPromo(CouponCodeDto couponCode)
    {
        var result = new CouponCodeValidationResult
        {
            CouponCodeId = couponCode.Id,
            Code = couponCode.Code,
            DiscountValue = couponCode.Value,
            DiscountType = couponCode.CouponCodeType,
            IsValid = true,
            ApplicableItems = new List<Guid>() // All items are applicable for cart-wide discounts
        };

        return ServiceResult<CouponCodeValidationResult>.SuccessResult(result);
    }

    private decimal CalculateCartDiscount(CouponCodeDto couponCode, decimal cartTotal)
    {
        return couponCode.CouponCodeType == CouponCodeType.Percentage
            ? cartTotal * (couponCode.Value / 100)
            : Math.Min(couponCode.Value, cartTotal);
    }
}
