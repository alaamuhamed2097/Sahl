using BL.Contracts.IMapper;
using BL.Contracts.Service.Warehouse;
using BL.Extensions;
using BL.Services.Base;
using Common.Filters;
using DAL.Contracts.Repositories;
using DAL.Models;
using DAL.Repositories;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Warehouse;
using Microsoft.AspNetCore.Identity;
using Resources;
using Serilog;
using Shared.DTOs.Vendor;
using Shared.DTOs.Warehouse;
using Shared.GeneralModels.SearchCriteriaModels;
using System.Linq.Expressions;

namespace BL.Services.Warehouse;

public class WarehouseService : BaseService<TbWarehouse, WarehouseDto>, IWarehouseService
{
    private readonly ITableRepository<TbWarehouse> _warehouseRepository;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly ITableRepository<TbVendor> _vendorRepository;
	private readonly ILogger _logger;
    private readonly IBaseMapper _mapper;

	public WarehouseService(
		ITableRepository<TbWarehouse> warehouseRepository,
		ILogger logger,
		IBaseMapper mapper,
		ITableRepository<TbVendor> vendorRepository,
		UserManager<ApplicationUser> userManager)
		: base(warehouseRepository, mapper)
	{
		_warehouseRepository = warehouseRepository;
		_logger = logger;
		_mapper = mapper;
		_vendorRepository = vendorRepository;
		_userManager = userManager;
	}

	public async Task<IEnumerable<WarehouseDto>> GetAllAsync()
    {
        var warehouses = await _warehouseRepository
            .GetAsync(x => !x.IsDeleted);

        return _mapper.MapList<TbWarehouse, WarehouseDto>(warehouses).ToList();
    }

    public async Task<IEnumerable<WarehouseDto>> GetActiveWarehousesAsync()
    {
        var warehouses = await _warehouseRepository
            .GetAsync(x => !x.IsDeleted && x.IsActive);

        return _mapper.MapList<TbWarehouse, WarehouseDto>(warehouses).ToList();
    }

    public async Task<WarehouseDto> GetMarketWarehousesAsync()
    {
        var warehouses = await _warehouseRepository
            .FindAsync(x => !x.IsDeleted && x.IsDefaultPlatformWarehouse);

        return _mapper.MapModel<TbWarehouse, WarehouseDto>(warehouses);
    }

    public async Task<WarehouseDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id));

        var warehouse = await _warehouseRepository.FindByIdAsync(id);
        if (warehouse == null) return null;

        return _mapper.MapModel<TbWarehouse, WarehouseDto>(warehouse);
    }
	//public async Task<PagedResult<WarehouseDto>> SearchAsync(BaseSearchCriteriaModel criteriaModel)
	//{
	//	if (criteriaModel == null)
	//		throw new ArgumentNullException(nameof(criteriaModel));

	//	if (criteriaModel.PageNumber < 1)
	//		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber),
	//			ValidationResources.PageNumberGreaterThanZero);

	//	if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
	//		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize),
	//			ValidationResources.PageSizeRange);

	//	// Base filter: غير محذوف
	//	Expression<Func<TbWarehouse, bool>> filter = x => !x.IsDeleted;

	//	// Search by term (Address or Email)
	//	var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
	//	if (!string.IsNullOrWhiteSpace(searchTerm))
	//	{
	//		filter = filter.And(x =>
	//			(x.Address != null && x.Address.ToLower().Contains(searchTerm)) ||
	//			(x.Email != null && x.Email.ToLower().Contains(searchTerm))
	//		);
	//	}

	//	// Filter by IsActive
	//	if (criteriaModel.IsActive.HasValue)
	//	{
	//		filter = filter.And(x => x.IsActive == criteriaModel.IsActive.Value);
	//	}

	//	// Filter by VendorId
	//	if (criteriaModel.VendorId.HasValue)
	//	{
	//		filter = filter.And(x => x.VendorId == criteriaModel.VendorId.Value);
	//	}

	//	// Filter by IsDefaultPlatformWarehouse
	//	if (criteriaModel.IsDefaultPlatformWarehouse.HasValue)
	//	{
	//		filter = filter.And(x => x.IsDefaultPlatformWarehouse == criteriaModel.IsDefaultPlatformWarehouse.Value);
	//	}

	//	// Get paged data with includes
	//	var warehouses = await _warehouseRepository.GetPageAsync(
	//		criteriaModel.PageNumber,
	//		criteriaModel.PageSize,
	//		filter,
	//		orderBy: q => q.OrderByDescending(x => x.CreatedDateUtc),
	//		includeProperties: "Vendor" // Include Vendor navigation property
	//	);

	//	// Map to DTOs
	//	var itemsDto = warehouses.Items.Select(w => new WarehouseDto
	//	{
	//		Id = w.Id,
	//		Address = w.Address,
	//		Email = w.Email,
	//		IsDefaultPlatformWarehouse = w.IsDefaultPlatformWarehouse,
	//		VendorId = w.VendorId,
	//		VendorName = w.Vendor?.Name, // احصل على اسم الـ Vendor
	//		IsActive = w.IsActive
	//	}).ToList();

	//	return new PagedResult<WarehouseDto>(itemsDto, warehouses.TotalRecords);
	//}

	public async Task<PagedResult<WarehouseDto>> SearchAsync(WarehouseSearchCriteriaModel criteriaModel)
	{
		if (criteriaModel == null)
			throw new ArgumentNullException(nameof(criteriaModel));

		if (criteriaModel.PageNumber < 1)
			throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber),
				ValidationResources.PageNumberGreaterThanZero);

		if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
			throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize),
				ValidationResources.PageSizeRange);

		Expression<Func<TbWarehouse, bool>> filter ;

				//filter = filter.And(x => x.IsDefaultPlatformWarehouse == true && x.VendorId == Guid.Empty);

		//if (criteriaModel.IsDefaultPlatformWarehouse.HasValue)
		//{
		//	if (criteriaModel.IsDefaultPlatformWarehouse.Value)
		//	{
		//		// Platform warehouses: IsDefaultPlatformWarehouse = true AND VendorId = Empty
		//	}
		//	else
		//	{
		//		// Vendor warehouses: IsDefaultPlatformWarehouse = false AND VendorId != Empty
		//		filter = filter.And(x => x.IsDefaultPlatformWarehouse == false && x.VendorId != Guid.Empty);
		//	}
		//}
		

		filter = x => !x.IsDeleted && x.IsDefaultPlatformWarehouse == true ;



		var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
		if (!string.IsNullOrWhiteSpace(searchTerm))
		{
			filter = filter.And(x =>
				(x.Address != null && x.Address.ToLower().Contains(searchTerm))
				//||
				//(x.Email != null && x.Email.ToLower().Contains(searchTerm))
			);
		}

		// Filter by IsActive
		if (criteriaModel.IsActive.HasValue)
		{
			filter = filter.And(x => x.IsActive == criteriaModel.IsActive.Value);
		}

		// Filter by IsDefaultPlatformWarehouse (Platform vs Vendor warehouse)
		if (criteriaModel.IsDefaultPlatformWarehouse.HasValue)
		{
			filter = filter.And(x => x.IsDefaultPlatformWarehouse == criteriaModel.IsDefaultPlatformWarehouse.Value);
		}

		// Filter by VendorId
		if (criteriaModel.VendorId.HasValue)
		{
			filter = filter.And(x => x.VendorId == criteriaModel.VendorId.Value);
		}

		// Filter by specific Email
		//if (!string.IsNullOrWhiteSpace(criteriaModel.Email))
		//{
		//	var email = criteriaModel.Email.Trim().ToLower();
		//	filter = filter.And(x => x.Email != null && x.Email.ToLower() == email);
		//}

		// Filter by specific Address
		if (!string.IsNullOrWhiteSpace(criteriaModel.Address))
		{
			var address = criteriaModel.Address.Trim().ToLower();
			filter = filter.And(x => x.Address != null && x.Address.ToLower().Contains(address));
		}

		// Filter by CreatedDateFrom
		if (criteriaModel.CreatedDateFrom.HasValue)
		{
			filter = filter.And(x => x.CreatedDateUtc >= criteriaModel.CreatedDateFrom.Value);
		}

		// Filter by CreatedDateTo
		if (criteriaModel.CreatedDateTo.HasValue)
		{
			filter = filter.And(x => x.CreatedDateUtc <= criteriaModel.CreatedDateTo.Value);
		}

		// Determine sort expression
		Func<IQueryable<TbWarehouse>, IOrderedQueryable<TbWarehouse>> orderBy = q =>
		{
			var sortBy = criteriaModel.SortBy?.Trim().ToLower();
			var ascending = criteriaModel.SortDirection?.ToLower() != "desc";

			return sortBy switch
			{
				"address" => ascending ? q.OrderBy(x => x.Address) : q.OrderByDescending(x => x.Address),
				//"email" => ascending ? q.OrderBy(x => x.Email) : q.OrderByDescending(x => x.Email),
				"isactive" => ascending ? q.OrderBy(x => x.IsActive) : q.OrderByDescending(x => x.IsActive),
				"createdate" => ascending ? q.OrderBy(x => x.CreatedDateUtc) : q.OrderByDescending(x => x.CreatedDateUtc),
				_ => q.OrderByDescending(x => x.CreatedDateUtc) // Default sort
			};
		};

		// Get paged data with includes
		var warehouses = await _warehouseRepository.GetPageAsync(
			criteriaModel.PageNumber,
			criteriaModel.PageSize,
			filter,
			orderBy: orderBy
			//includeProperties: "Vendor"
		);

		// Map to DTOs
		var itemsDto = warehouses.Items.Select(w => new WarehouseDto
		{
			Id = w.Id,
			Address = w.Address,
			//Email = w.Email,
			IsDefaultPlatformWarehouse = w.IsDefaultPlatformWarehouse,
			VendorId = w.VendorId,
			//VendorName = w.Vendor?.Name,
			IsActive = w.IsActive
		}).ToList();

		return new PagedResult<WarehouseDto>(itemsDto, warehouses.TotalRecords);
	}

	public async Task<PagedResult<WarehouseDto>> SearchVendorAsync(WarehouseSearchCriteriaModel criteriaModel)
	{
		if (criteriaModel == null)
			throw new ArgumentNullException(nameof(criteriaModel));

		if (criteriaModel.PageNumber < 1)
			throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber),
				ValidationResources.PageNumberGreaterThanZero);

		if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
			throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize),
				ValidationResources.PageSizeRange);

		Expression<Func<TbWarehouse, bool>> filter = x => !x.IsDeleted;

				filter = filter.And(x => x.IsDefaultPlatformWarehouse == false && x.VendorId != Guid.Empty);

		//if (criteriaModel.IsDefaultPlatformWarehouse.HasValue)
		//{
		//	if (criteriaModel.IsDefaultPlatformWarehouse.Value)
		//	{
		//		// Platform warehouses: IsDefaultPlatformWarehouse = true AND VendorId = Empty
		//		filter = filter.And(x => x.IsDefaultPlatformWarehouse == true && x.VendorId == Guid.Empty);
		//	}
		//	else
		//	{
		//		// Vendor warehouses: IsDefaultPlatformWarehouse = false AND VendorId != Empty
		//	}
		//}


		//filter = x => !x.IsDeleted && x.IsDefaultPlatformWarehouse == true && x.VendorId == Guid.Empty;



		var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
		if (!string.IsNullOrWhiteSpace(searchTerm))
		{
			filter = filter.And(x =>
				(x.Address != null && x.Address.ToLower().Contains(searchTerm))
			//||
			//(x.Email != null && x.Email.ToLower().Contains(searchTerm))
			);
		}

		// Filter by IsActive
		if (criteriaModel.IsActive.HasValue)
		{
			filter = filter.And(x => x.IsActive == criteriaModel.IsActive.Value);
		}

		// Filter by IsDefaultPlatformWarehouse (Platform vs Vendor warehouse)
		if (criteriaModel.IsDefaultPlatformWarehouse.HasValue)
		{
			filter = filter.And(x => x.IsDefaultPlatformWarehouse == criteriaModel.IsDefaultPlatformWarehouse.Value);
		}

		// Filter by VendorId
		if (criteriaModel.VendorId.HasValue)
		{
			filter = filter.And(x => x.VendorId == criteriaModel.VendorId.Value);
		}

		// Filter by specific Email
		//if (!string.IsNullOrWhiteSpace(criteriaModel.Email))
		//{
		//	var email = criteriaModel.Email.Trim().ToLower();
		//	filter = filter.And(x => x.Email != null && x.Email.ToLower() == email);
		//}

		// Filter by specific Address
		if (!string.IsNullOrWhiteSpace(criteriaModel.Address))
		{
			var address = criteriaModel.Address.Trim().ToLower();
			filter = filter.And(x => x.Address != null && x.Address.ToLower().Contains(address));
		}

		// Filter by CreatedDateFrom
		if (criteriaModel.CreatedDateFrom.HasValue)
		{
			filter = filter.And(x => x.CreatedDateUtc >= criteriaModel.CreatedDateFrom.Value);
		}

		// Filter by CreatedDateTo
		if (criteriaModel.CreatedDateTo.HasValue)
		{
			filter = filter.And(x => x.CreatedDateUtc <= criteriaModel.CreatedDateTo.Value);
		}

		// Determine sort expression
		Func<IQueryable<TbWarehouse>, IOrderedQueryable<TbWarehouse>> orderBy = q =>
		{
			var sortBy = criteriaModel.SortBy?.Trim().ToLower();
			var ascending = criteriaModel.SortDirection?.ToLower() != "desc";

			return sortBy switch
			{
				"address" => ascending ? q.OrderBy(x => x.Address) : q.OrderByDescending(x => x.Address),
				//"email" => ascending ? q.OrderBy(x => x.Email) : q.OrderByDescending(x => x.Email),
				"isactive" => ascending ? q.OrderBy(x => x.IsActive) : q.OrderByDescending(x => x.IsActive),
				"createdate" => ascending ? q.OrderBy(x => x.CreatedDateUtc) : q.OrderByDescending(x => x.CreatedDateUtc),
				_ => q.OrderByDescending(x => x.CreatedDateUtc) // Default sort
			};
		};

		// Get paged data with includes
		var warehouses = await _warehouseRepository.GetPageAsync(
			criteriaModel.PageNumber,
			criteriaModel.PageSize,
			filter,
			orderBy: orderBy
		//includeProperties: "Vendor"
		);

		// Map to DTOs
		var itemsDto = warehouses.Items.Select(w => new WarehouseDto
		{
			Id = w.Id,
			Address = w.Address,
			//Email = w.Email,
			IsDefaultPlatformWarehouse = w.IsDefaultPlatformWarehouse,
			VendorId = w.VendorId,
			//VendorName = w.Vendor?.Name,
			IsActive = w.IsActive
		}).ToList();

		return new PagedResult<WarehouseDto>(itemsDto, warehouses.TotalRecords);
	}



	//public async Task<PagedResult<WarehouseDto>> SearchAsync(WarehouseSearchCriteriaModel criteriaModel)
	//{
	//	if (criteriaModel == null)
	//		throw new ArgumentNullException(nameof(criteriaModel));

	//	if (criteriaModel.PageNumber < 1)
	//		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber), ValidationResources.PageNumberGreaterThanZero);

	//	if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
	//		throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize), ValidationResources.PageSizeRange);

	//	Expression<Func<TbWarehouse, bool>> filter = x => !x.IsDeleted;

	//	var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
	//	if (!string.IsNullOrWhiteSpace(searchTerm))
	//	{
	//		filter = filter.And(x =>
	//			//x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm) ||
	//			//x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm) ||
	//			x.Address != null && x.Address.ToLower().Contains(searchTerm)
	//		);
	//	}

	//	var warehouses = await _warehouseRepository.GetPageAsync(
	//		criteriaModel.PageNumber,
	//		criteriaModel.PageSize,
	//		filter,
	//		orderBy: q => q.OrderByDescending(x => x.CreatedDateUtc));

	//	var itemsDto = _mapper.MapList<TbWarehouse, WarehouseDto>(warehouses.Items).ToList();

	//	return new PagedResult<WarehouseDto>(itemsDto, warehouses.TotalRecords);
	//}


	public async Task<IEnumerable<VendorDto>> GetVendorsAsync()
	{
		try
		{
			
			var vendors = await _vendorRepository.GetAsync(x => !x.IsDeleted );

			return vendors.Select(v => new VendorDto
			{
				Id = v.Id,
				UserId = v.UserId,

				
			}).ToList();
		}
		catch (Exception ex)
		{
			_logger.Error(ex, "Error getting vendors");
			return Enumerable.Empty<VendorDto>();
		}
	}

	public async Task<IEnumerable<VendorWithUserDto>> GetVendorUsersAsync()
	{
		try
		{
			var vendors = await _vendorRepository.GetAsync(x => !x.IsDeleted);

			//var vendorByUserId = vendors
			//	.Where(v => !string.IsNullOrEmpty(v.UserId))
			//	.ToDictionary(v => v.UserId, v => v.Id);
			var vendorByUserId = vendors
	.Where(v => !string.IsNullOrEmpty(v.UserId))
	.GroupBy(v => v.UserId)
	.ToDictionary(g => g.Key, g => g.First().Id);

			var users = new List<ApplicationUser>();
			foreach (var userId in vendorByUserId.Keys)
			{
				var user = await _userManager.FindByIdAsync(userId);
				if (user != null)
				{
					users.Add(user);
				}
			}

			return users.Select(u =>
			{
				

				return new VendorWithUserDto
				{
					VendorId = vendorByUserId[u.Id], 
					UserId = new Guid(u.Id),               
					UserName = u.UserName,
					Email = u.Email,
					PhoneNumber = u.PhoneNumber
				};
			}).ToList();
		}
		catch (Exception ex)
		{
			_logger.Error(ex, "Error getting vendor users");
			return Enumerable.Empty<VendorWithUserDto>();
		}
	}

	public async Task<bool> IsMultiVendorEnabledAsync()
	{
		try
		{
			// افترض أن عندك SystemSettings table أو config
			// يمكنك استبدال هذا بطريقتك في تخزين الـ settings

			// مثال:
			// var setting = await _systemSettingsRepository
			//     .FirstOrDefaultAsync(x => x.Key == "MultiVendorEnabled");
			// return setting?.Value == "true";

			// أو لو عندك في appsettings.json:
			// return _configuration.GetValue<bool>("Features:MultiVendorEnabled");

			// مؤقتاً، نرجع true للتجربة
			return await Task.FromResult(true);
		}
		catch (Exception ex)
		{
			_logger.Error(ex, "Error checking multi-vendor status");
			return false;
		}
	}
	public async Task<bool> SaveAsync(WarehouseDto dto, Guid userId)
    {
		TbWarehouse? isExist = null;

		if (dto.VendorId.HasValue)
		{
			isExist = await _warehouseRepository.FindAsync(x => x.VendorId == dto.VendorId.Value);
		}
		if (isExist != null)
			return false;

		var entity = _mapper.MapModel<WarehouseDto, TbWarehouse>(dto);
        var result = await _warehouseRepository.SaveAsync(entity, userId);
        return result.Success;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        return await _warehouseRepository.UpdateCurrentStateAsync(id, userId, true);
    }

    public async Task<bool> ToggleActiveStatusAsync(Guid id, Guid userId)
    {
        var warehouse = await _warehouseRepository.FindByIdAsync(id);
        if (warehouse == null)
            throw new KeyNotFoundException($"Warehouse with ID {id} not found");

        warehouse.IsActive = !warehouse.IsActive;
        var result = await _warehouseRepository.UpdateAsync(warehouse, userId);
        return result.Success;
    }
}
